using Catch22.Development.Common;
using Catch22.Development.Theatre.Library.Concrete;
using Catch22.Development.Theatre.Library.Domain;
using Catch22.Development.Theatre.Library.DTO;
using Catch22.Development.Theatre.Library.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using XmlShow = Catch22.Development.Theatre.Library.DTO.XmlShow;

namespace Catch22.Development.Theatre.Library.Helpers
{
    public class DBHelper
    {
        private readonly IFacilitiesRepository _facilitiesRepository;
        private readonly IGroupRatesRepository _groupRatesRepository;
        private readonly IJsonParser _jsonParser;
        private readonly IMealDealsRepository _mealDealsRepository;
        private readonly IRestaurantsRepository _restaurantsRepository;
        private readonly IRestaurantImagesRepository _restaurantImagesRepository;
        private readonly IShowDatesRepository _showDatesRepository;
        private readonly IShowsRepository _showsRepository;
        private readonly IShowSchedulesRepository _showSchedulesRepository;
        private readonly ISpecialOffersRepository _specialOffersRepository;
        public StringBuilder Status { get; private set; }
        private readonly IVenuesRepository _venuesRepository;
        private readonly IXmlParser _xmlParser;

        public DBHelper(bool downloadXmlFeed, IFacilitiesRepository facilitiesRepository, IGroupRatesRepository groupRatesRepository, IJsonParser jsonParser, IMealDealsRepository mealDealsRepository, IRestaurantsRepository restaurantsRepository, IRestaurantImagesRepository restaurantImagesRepository, IShowDatesRepository showDatesRepository, IShowsRepository showsRepository, IShowSchedulesRepository showSchedulesRepository, ISpecialOffersRepository specialOffersRepository, IVenuesRepository venuesRepository, IXmlParser xmlParser)
        {
            _facilitiesRepository = facilitiesRepository;
            _groupRatesRepository = groupRatesRepository;
            _jsonParser = jsonParser;
            _mealDealsRepository = mealDealsRepository;
            _restaurantsRepository = restaurantsRepository;
            _restaurantImagesRepository = restaurantImagesRepository;
            _showDatesRepository = showDatesRepository;
            _showsRepository = showsRepository;
            _showSchedulesRepository = showSchedulesRepository;
            _specialOffersRepository = specialOffersRepository;
            _venuesRepository = venuesRepository;
            _xmlParser = xmlParser;

            if (downloadXmlFeed)
                _xmlParser.DownloadXmlFeed();

            Status = new StringBuilder();
        }

        #region Facilities

        public Facility AddFacility(string description)
        {
            var facility = new Facility
            {
                Description = description,
                Active = true
            };

            _facilitiesRepository.Insert(facility);
            _facilitiesRepository.Save();

            return facility;
        }

        #endregion

        #region GroupRates

        public void UpdateGroupRates()
        {
            var xmlGroupRates = _xmlParser.GetGroupRates();
            var groupRatesAdded = 0;

            if (xmlGroupRates.Any())
            {
                _groupRatesRepository.ExecuteSqlCommand("Delete From GroupRateDay");
                _groupRatesRepository.ExecuteSqlCommand("Delete From GroupRate");

                foreach (var xmlGroupRate in xmlGroupRates)
                {
                    var groupRate = new GroupRate { MerchantGroupRateId = xmlGroupRate.MerchantGroupRateId };
                    var show = _showsRepository.FirstOrDefault(s => s.MerchantShowId == xmlGroupRate.MerchantShowId);

                    if (show != null)
                    {
                        groupRate.ShowId = show.Id;
                        groupRate.Name = xmlGroupRate.Name;
                        groupRate.FaceValue = xmlGroupRate.FaceValue;
                        groupRate.SaleValue = xmlGroupRate.Price;
                        groupRate.MinimumTicketQuantity = xmlGroupRate.MinimumTicketQuantity;
                        groupRate.BookingFrom = xmlGroupRate.BookingFrom;
                        groupRate.BookingTo = xmlGroupRate.BookingTo;
                        groupRate.BookingBy = xmlGroupRate.BookingBy;
                        groupRate.ExclusionsValidity = xmlGroupRate.ExclusionsValidity;
                        AddGroupRateDays(xmlGroupRate, groupRate);
                        groupRate.Active = true;
                        _groupRatesRepository.Insert(groupRate);
                        groupRatesAdded++;
                    }
                }

                _groupRatesRepository.Save();
            }

            Status.AppendLine(string.Format("<p>Group Rates Added: {0}</p>", groupRatesAdded));
        }

        private static void AddGroupRateDays(XmlGroupRate xmlGroupRate, GroupRate groupRate)
        {
            foreach (var xmlGroupRateDay in xmlGroupRate.XmlGroupRateDays)
            {
                var groupRateDay = new GroupRateDay { Day = xmlGroupRateDay.Day, Type = xmlGroupRateDay.Type };

                groupRate.GroupRateDays.Add(groupRateDay);
            }
        }

        #endregion

        #region MealDeals

        public void UpdateMealDeals()
        {
            var xmlMealDeals = _xmlParser.GetMealDeals();
            var mealDealsAdded = 0;

            if (xmlMealDeals.Count > 0)
            {
                _mealDealsRepository.ExecuteSqlCommand("Delete From MealDeal");

                foreach (var xmlMealDeal in xmlMealDeals)
                {
                    if (ProcessErrors(xmlMealDeal))
                        continue;

                    var mealDeal = new MealDeal();
                    var show = _showsRepository.FirstOrDefault(s => s.MerchantShowId == xmlMealDeal.MerchantShowId);
                    var restaurant = _restaurantsRepository.FirstOrDefault(r => r.MerchantRestaurantId == xmlMealDeal.MerchantRestaurantId);

                    if (show != null && restaurant != null)
                    {
                        mealDeal.ShowId = show.Id;
                        mealDeal.RestaurantId = restaurant.Id;

                        if (xmlMealDeal.MealDealType == XmlMealDealTypes.Pre_Theatre)
                            mealDeal.MealDealTypeId = (byte)MealDealTypes.Pre_Theatre;
                        else
                            mealDeal.MealDealTypeId = (byte)MealDealTypes.Post_Theatre;

                        mealDeal.Title = xmlMealDeal.Title;
                        mealDeal.Description = xmlMealDeal.Description;
                        mealDeal.MealTime = xmlMealDeal.MealTime;
                        mealDeal.ShowTime = xmlMealDeal.ShowTime;
                        mealDeal.NormalPrice = xmlMealDeal.NormalPrice;
                        mealDeal.PackagePrice = xmlMealDeal.PackagePrice;
                        mealDeal.Conditions = xmlMealDeal.Conditions;
                        mealDeal.Exclusions = xmlMealDeal.Exclusions;
                        mealDeal.AffiliateUrl = xmlMealDeal.AffiliateUrl;
                        mealDeal.Active = true;
                        _mealDealsRepository.Insert(mealDeal);
                        mealDealsAdded++;
                    }
                }

                _mealDealsRepository.Save();
            }

            Status.AppendLine(string.Format("<p>Meal Deals Added: {0}</p>", mealDealsAdded));
        }

        #endregion

        #region Restaurants

        public void UpdateRestaurants()
        {
            var xmlRestaurants = _xmlParser.GetRestaurants();
            var restaurantsAdded = 0;

            foreach (var xmlRestaurant in xmlRestaurants)
            {
                if (ProcessErrors(xmlRestaurant))
                    continue;

                var restaurant = _restaurantsRepository.FirstOrDefault(r => r.MerchantRestaurantId == xmlRestaurant.MerchantRestaurantId);

                if (restaurant == null)
                {
                    restaurant = AddRestaurant(xmlRestaurant);
                    _restaurantsRepository.Insert(restaurant);
                    restaurantsAdded++;
                }
                else
                {
                    restaurant.Name = string.IsNullOrEmpty(xmlRestaurant.Name) ? restaurant.Name : xmlRestaurant.Name;
                    restaurant.Description = string.IsNullOrEmpty(xmlRestaurant.Description) ? restaurant.Description : xmlRestaurant.Description;
                    restaurant.MinimumPrice = xmlRestaurant.MinimumPrice;
                    restaurant.Address1 = string.IsNullOrEmpty(xmlRestaurant.Address1) ? restaurant.Address1 : xmlRestaurant.Address1;
                    restaurant.Address2 = string.IsNullOrEmpty(xmlRestaurant.Address2) ? restaurant.Address2 : xmlRestaurant.Address2;
                    restaurant.Postcode = string.IsNullOrEmpty(xmlRestaurant.Postcode) ? restaurant.Postcode : xmlRestaurant.Postcode;

                    if (!restaurant.Latitude.HasValue && !restaurant.Longitude.HasValue)
                    {
                        restaurant.Latitude = xmlRestaurant.Latitude;
                        restaurant.Longitude = xmlRestaurant.Longitude;
                    }

                    restaurant.NearestTube = string.IsNullOrEmpty(xmlRestaurant.NearestTube) ? restaurant.NearestTube : xmlRestaurant.NearestTube;
                    restaurant.TubeDirection = string.IsNullOrEmpty(xmlRestaurant.TubeDirection) ? restaurant.TubeDirection : xmlRestaurant.TubeDirection;
                    restaurant.Underground = string.IsNullOrEmpty(xmlRestaurant.Underground) ? restaurant.Underground : xmlRestaurant.Underground;
                    restaurant.RailwayStation = string.IsNullOrEmpty(xmlRestaurant.RailwayStation) ? restaurant.RailwayStation : xmlRestaurant.RailwayStation;
                    restaurant.BusRoutes = string.IsNullOrEmpty(xmlRestaurant.BusRoutes) ? restaurant.BusRoutes : xmlRestaurant.BusRoutes;
                    restaurant.NightBusRoutes = string.IsNullOrEmpty(xmlRestaurant.NightBusRoutes) ? restaurant.NightBusRoutes : xmlRestaurant.NightBusRoutes;
                    restaurant.CarPark = string.IsNullOrEmpty(xmlRestaurant.CarPark) ? restaurant.CarPark : xmlRestaurant.CarPark;
                    restaurant.InCongestionZone = string.IsNullOrEmpty(xmlRestaurant.InCongestionZone) ? restaurant.InCongestionZone : xmlRestaurant.InCongestionZone;
                    restaurant.TravelInstructions = string.IsNullOrEmpty(xmlRestaurant.TravelInstructions) ? restaurant.TravelInstructions : xmlRestaurant.TravelInstructions;
                    UpdateRestaurantImages(xmlRestaurant, restaurant);
                    UpdateRestaurantFacilities(xmlRestaurant, restaurant);
                }
            }

            _restaurantsRepository.Save();

            Status.AppendLine(string.Format("<p>Venues Added: {0}</p>", restaurantsAdded));
        }

        private Restaurant AddRestaurant(XmlRestaurant xmlRestaurant)
        {
            var restaurant = new Restaurant
            {
                MerchantRestaurantId = xmlRestaurant.MerchantRestaurantId,
                Slug = CommonFunctions.GetFriendlyUrl(xmlRestaurant.Name),
                Name = xmlRestaurant.Name,
                Description = xmlRestaurant.Description,
                MinimumPrice = xmlRestaurant.MinimumPrice,
                Address1 = xmlRestaurant.Address1,
                Address2 = xmlRestaurant.Address2,
                Postcode = xmlRestaurant.Postcode,
                Latitude = xmlRestaurant.Latitude,
                Longitude = xmlRestaurant.Longitude,
                NearestTube = xmlRestaurant.NearestTube,
                TubeDirection = xmlRestaurant.TubeDirection,
                Underground = xmlRestaurant.Underground,
                RailwayStation = xmlRestaurant.RailwayStation,
                BusRoutes = xmlRestaurant.BusRoutes,
                NightBusRoutes = xmlRestaurant.NightBusRoutes,
                CarPark = xmlRestaurant.CarPark,
                InCongestionZone = xmlRestaurant.InCongestionZone,
                TravelInstructions = xmlRestaurant.TravelInstructions,
                Active = true
            };

            AddRestaurantImages(xmlRestaurant, restaurant);
            AddRestaurantFacilities(xmlRestaurant.XmlFacilities, restaurant);

            return restaurant;
        }

        private void AddRestaurantFacilities(IEnumerable<string> facilities, Restaurant restaurant)
        {
            foreach (var facilityToAdd in facilities)
            {
                var facility = _facilitiesRepository.FirstOrDefault(f => f.Description == facilityToAdd) ?? AddFacility(facilityToAdd);

                if (facility != null && facility.Id > 0)
                    restaurant.Facilities.Add(facility);
            }
        }

        private static void AddRestaurantImages(XmlRestaurant xmlRestaurant, Restaurant restaurant)
        {
            foreach (var xmlImage in xmlRestaurant.XmlImages)
            {
                var restaurantImage = new RestaurantImage
                {
                    ImageTypeId = (byte)xmlImage.ImageType,
                    Filename = xmlImage.ImageUrl,
                    Modified = true,
                    Active = true
                };

                restaurant.RestaurantImages.Add(restaurantImage);
            }
        }

        private void UpdateRestaurantFacilities(XmlRestaurant xmlRestaurant, Restaurant restaurant)
        {
            var facilitiesToDelete = restaurant.Facilities
                .Where(fd => !xmlRestaurant.XmlFacilities.Contains(fd.Description)).ToList();

            var facilitiesToAdd = xmlRestaurant.XmlFacilities
                .Where(fa => !restaurant.Facilities.Select(vf => vf.Description).ToList().Contains(fa));

            facilitiesToDelete.ForEach(fd => restaurant.Facilities.Remove(fd));

            AddRestaurantFacilities(facilitiesToAdd, restaurant);
        }

        private void UpdateRestaurantImages(XmlRestaurant xmlRestaurant, Restaurant restaurant)
        {
            if (xmlRestaurant.XmlImages.Any())
            {
                _restaurantImagesRepository.ExecuteSqlCommand("DELETE FROM RestaurantImage WHERE RestaurantId = " + restaurant.Id);

                foreach (var xmlImage in xmlRestaurant.XmlImages)
                {
                    var restaurantImage = new RestaurantImage
                    {
                        ImageTypeId = (byte)xmlImage.ImageType,
                        Filename = xmlImage.ImageUrl,
                        Modified = true,
                        Active = true
                    };

                    restaurant.RestaurantImages.Add(restaurantImage);
                }
            }

            //var restaurantImageFileNames = GetImageFileNames(restaurant.RestaurantImages);

            //foreach (var xmlImage in xmlRestaurant.XmlImages)
            //{
            //    var fileName = CommonFunctions.GetFileName(xmlImage.ImageUrl);

            //    if (!restaurantImageFileNames.Contains(fileName))
            //    {
            //        restaurantImageFileNames.Add(fileName);

            //        var restaurantImage = new RestaurantImage
            //        {
            //            ImageTypeId = (byte)xmlImage.ImageType,
            //            Filename = xmlImage.ImageUrl,
            //            Modified = true,
            //            Active = true
            //        };

            //        restaurant.RestaurantImages.Add(restaurantImage);
            //    }
            //}
        }

        #endregion

        #region Seating Plans

        public void UpdateSeatingPlans()
        {
            var shows = _showsRepository.AllIncluding(s => s.Venue).Where(s => s.ShowStatusId == 2 && s.Active);
            var seatingPlansAdded = 0;

            foreach (var show in shows)
            {
                var jsonSeatingPlan = _jsonParser.GetSeatingPlan(show.Venue.LocationCode, show.MerchantShowId);

                if (!ProcessErrors(jsonSeatingPlan))
                {
                    DoUpdateSeatingPlan(show, jsonSeatingPlan);
                    seatingPlansAdded++;
                }
            }

            _showsRepository.Save();
            Status.AppendLine(string.Format("<p>Seating Plans Added/Updated: {0}</p>", seatingPlansAdded));
        }

        private static void DoUpdateSeatingPlan(Show show, JsonSeatingPlan jsonSeatingPlan)
        {
            var seatingPlan = new SeatingPlan();

            if (jsonSeatingPlan.blocks.Any())
            {
                foreach (var block in jsonSeatingPlan.blocks)
                {
                    if (block.name.ToLower() == "stage") continue;

                    var spRowLabels = new List<SpRowLabel>();

                    foreach (var rowLabel in block.rowLabels)
                    {
                        spRowLabels.Add(new SpRowLabel
                        {
                            X = Convert.ToInt32(rowLabel[0]),
                            Y = Convert.ToInt32(rowLabel[1]) * 1 + 3,
                            Label = rowLabel[2]
                        });
                    }

                    var spSeats = new List<SpSeat>();

                    foreach (var subBlock in block.subBlocks)
                    {
                        foreach (var seat in subBlock.seats)
                        {
                            spSeats.Add(new SpSeat
                            {
                                X = Convert.ToInt32(seat[2]) * 1 + 4,
                                Y = Convert.ToInt32(seat[3]),
                                Number = Convert.ToInt32(seat[5]),
                                Row = seat[6]
                            });
                        }
                    }

                    seatingPlan.SpBlocks.Add(new SpBlock
                    {
                        Height = Math.Max(spRowLabels.Max(l => l.Y), spSeats.Max(s => s.Y)),
                        Width = Math.Max(spRowLabels.Max(l => l.X), spSeats.Max(s => s.X)),
                        Ids = block.ThebsBlockIds.Split(',').ToList(),
                        Name = block.name,
                        SpRowLabels = spRowLabels,
                        SpSeats = spSeats,
                    });
                }

                seatingPlan.AdjustCoordinates();
                seatingPlan.CalculateBlockOffsets();
                show.SeatingPlan = JsonConvert.SerializeObject(seatingPlan);
            }
        }        

        #endregion

        #region Show Dates

        public void UpdateShowDates()
        {
            var today = DateTime.Now.Date;
            var showDates = _showDatesRepository.AllWhere(sd => sd.Date >= today);
            var shows = _showsRepository.AllIncluding(s => s.Venue).Where(s => s.ShowStatusId == 2 && s.Active);
            var xmlShowDates = _xmlParser.GetShowDates(shows);
            var showDatesAdded = 0;

            if (xmlShowDates.Any())
            {
                _showDatesRepository.ExecuteSqlCommand("DELETE FROM ShowDate WHERE Date < CAST(FLOOR(CAST(GETDATE() AS FLOAT)) AS DATETIME)");

                var xmlShowDateGroups = xmlShowDates
                    .OrderBy(sd => sd.MerchantShowId)
                    .ThenBy(sd => sd.Date)
                    .GroupBy(x => new { x.MerchantShowId })
                    .Select(x => new { x.Key.MerchantShowId, Dates = x.Select(y => new XmlShowDate { PerformanceId = y.PerformanceId, Date = y.Date }) });

                foreach (var xmlShowDateGroup in xmlShowDateGroups)
                {
                    var show = shows.FirstOrDefault(s => s.MerchantShowId == xmlShowDateGroup.MerchantShowId);

                    if (show == null) continue;

                    foreach (var xmlShowDate in xmlShowDateGroup.Dates)
                    {
                        if (!showDates.Any(sd => sd.ShowId == show.Id && sd.Date == xmlShowDate.Date))
                        {
                            var showDate = new ShowDate { PerformanceId = xmlShowDate.PerformanceId, ShowId = show.Id, Date = xmlShowDate.Date };

                            _showDatesRepository.Insert(showDate);
                            showDatesAdded++;
                        }
                    }
                }

                _showDatesRepository.Save();
            }

            Status.AppendLine(string.Format("<p>Show Dates Added: {0}</p>", showDatesAdded));
        }

        #endregion

        #region Shows

        public void UpdateShow(int Id)
        {
            var show = _showsRepository.Find(Id);
            var xmlShow = _xmlParser.GetShow(show.Venue.LocationCode, show.MerchantShowId);

            if (ProcessErrors(xmlShow))
                throw new ApplicationException(Status.ToString());

            UpdateShow(show, xmlShow);
            _showsRepository.Save();
        }

        public void UpdateShows(bool fullUpdate)
        {
            var xmlShows = _xmlParser.GetShows();
            var showsAdded = 0;

            foreach (var xmlShow in xmlShows)
            {
                if (ProcessErrors(xmlShow))
                    continue;

                var show = _showsRepository.FirstOrDefault(s => s.MerchantShowId == xmlShow.MerchantShowId);

                if (show == null)
                {
                    show = AddShow(xmlShow);
                    _showsRepository.Insert(show);
                    _showsRepository.Save();
                    showsAdded++;
                }
                else if (fullUpdate)
                {
                    UpdateShow(show, xmlShow);
                    _showsRepository.Save();
                }
            }

            Status.AppendLine(string.Format("<p>Shows Added: {0}</p>", showsAdded));
        }

        private Show AddShow(XmlShow xmlShow)
        {
            var show = new Show
            {
                MerchantShowId = xmlShow.MerchantShowId,
                Name = xmlShow.Name,
                Slug = CommonFunctions.GetFriendlyUrl(xmlShow.Name).ToLower(),
                BookingFrom = xmlShow.BookingFrom,
                BookingUntil = xmlShow.BookingUntil,
                Runtime = xmlShow.Runtime,
                Matinee = xmlShow.Matinee,
                Evenings = xmlShow.Evenings,
                AgeRestriction = CommonFunctions.StripHtmlTags(xmlShow.AgeRestriction),
                ImportantInfo = CommonFunctions.StripHtmlTags(xmlShow.ImportantInfo),
                ShowStatusId = (byte)ShowStatuses.Live,
            };

            switch (xmlShow.Category)
            {
                case XmlCategories.Musicals:
                    show.ShowCategoryId = (byte)ShowCategories.Musicals;
                    break;
                case XmlCategories.Plays:
                    show.ShowCategoryId = (byte)ShowCategories.Plays;
                    break;
            }

            show.VenueId = _venuesRepository.Single(v => v.MerchantVenueId == xmlShow.MerchantVenueId).Id;

            var xmlImages = xmlShow.XmlImages.Where(i => i.ImageUrl.ToLower().StartsWith("http") || i.ImageUrl.ToLower().StartsWith("https"));

            foreach (var xmlImage in xmlImages)
            {
                var showImage = new ShowImage
                {
                    Alt = xmlImage.Alt,
                    Filename = xmlImage.ImageUrl,
                    ImageTypeId = EnsureUniqueCoverImage(xmlImage.ImageType, show),
                    Modified = true
                };

                show.ShowImages.Add(showImage);
            }

            var xmlVideo = xmlShow.XmlVideos.FirstOrDefault();

            if (xmlVideo != null)
            {
                var showVideo = new ShowVideo
                {
                    Filename = xmlVideo.Url,
                    Modified = true
                };

                show.ShowVideos.Add(showVideo);
            }

            return show;
        }

        private void UpdateShow(Show show, XmlShow xmlShow)
        {
            show.Name = string.IsNullOrEmpty(xmlShow.Name) ? show.Name : xmlShow.Name;
            show.BookingFrom = xmlShow.BookingFrom;
            show.BookingUntil = xmlShow.BookingUntil;
            show.Runtime = xmlShow.Runtime;
            show.Matinee = xmlShow.Matinee;
            show.Evenings = xmlShow.Evenings;
            show.AgeRestriction = CommonFunctions.StripHtmlTags(xmlShow.AgeRestriction);
            show.ImportantInfo = CommonFunctions.StripHtmlTags(xmlShow.ImportantInfo);
            show.VenueId = _venuesRepository.Single(v => v.MerchantVenueId == xmlShow.MerchantVenueId).Id;

            var showImageFileNames = GetImageFileNames(show.ShowImages);
            var xmlImages = xmlShow.XmlImages.Where(i => i.ImageUrl.ToLower().StartsWith("http"));

            foreach (var xmlImage in xmlImages)
            {
                var fileName = CommonFunctions.GetFileName(xmlImage.ImageUrl);

                if (!showImageFileNames.Contains(fileName))
                {
                    // This ensures we dont add the same image twice
                    showImageFileNames.Add(fileName);

                    var showImage = new ShowImage
                    {
                        Alt = xmlImage.Alt,
                        Filename = xmlImage.ImageUrl,
                        ImageTypeId = EnsureUniqueCoverImage(xmlImage.ImageType, show),
                        Modified = true
                    };

                    show.ShowImages.Add(showImage);
                    show.Modified = true;
                }
            }

            var xmlVideo = xmlShow.XmlVideos.FirstOrDefault();

            if (xmlVideo != null)
            {
                if (show.ShowVideos.All(sv => sv.Filename != xmlVideo.Url))
                {
                    var showVideo = new ShowVideo
                    {
                        Filename = xmlVideo.Url,
                        Modified = true
                    };

                    show.ShowVideos.Add(showVideo);
                }
            }
        }

        public void UpdateShowStatus()
        {
            var shows = _showsRepository.All();

            foreach (var show in shows.Where(show => show.BookingUntil.HasValue))
                show.ShowStatusId = show.BookingUntil >= DateTime.Now.Date ? (byte)ShowStatuses.Live : (byte)ShowStatuses.Finished;

            _showsRepository.Save();
        }

        #endregion

        #region ShowSchedules

        public void UpdateShowSchedules()
        {
            var xmlShowSchedules = _xmlParser.GetShowSchedules();
            var showSchedulesAdded = 0;

            if (xmlShowSchedules.Count > 0)
            {
                _showSchedulesRepository.ExecuteSqlCommand("Delete From ShowSchedule");

                foreach (var xmlShowSchedule in xmlShowSchedules)
                {
                    var showSchedule = new ShowSchedule();
                    var show = _showsRepository.FirstOrDefault(s => s.MerchantShowId == xmlShowSchedule.MerchantShowId);

                    if (show != null)
                    {
                        showSchedule.ShowId = show.Id;
                        showSchedule.Day = xmlShowSchedule.Day;
                        showSchedule.Time = xmlShowSchedule.Time;
                        _showSchedulesRepository.Insert(showSchedule);
                        showSchedulesAdded++;
                    }
                }

                _showSchedulesRepository.Save();
            }

            Status.AppendLine(string.Format("<p>Show Schedules Added: {0}</p>", showSchedulesAdded));
        }

        #endregion

        #region Special Offers

        public void UpdateSpecialOffers()
        {
            var xmlSpecialOffers = _xmlParser.GetSpecialOffers();
            var specialOffersAdded = 0;

            if (xmlSpecialOffers.Count > 0)
            {
                _specialOffersRepository.ExecuteSqlCommand("Delete From SpecialOffer");

                foreach (var xmlSpecialOffer in xmlSpecialOffers)
                {
                    var specialOffer = new SpecialOffer();
                    var show = _showsRepository.FirstOrDefault(s => s.MerchantShowId == xmlSpecialOffer.MerchantShowId);

                    if (show != null)
                    {
                        specialOffer.ShowId = show.Id;
                        specialOffer.Title = xmlSpecialOffer.Title;
                        specialOffer.NormalPrice = xmlSpecialOffer.NormalPrice;
                        specialOffer.OfferPrice = xmlSpecialOffer.OfferPrice;
                        specialOffer.Conditions = xmlSpecialOffer.Conditions;
                        specialOffer.Exclusions = xmlSpecialOffer.Exclusions;
                        specialOffer.Active = true;
                        _specialOffersRepository.Insert(specialOffer);
                        specialOffersAdded++;
                    }
                }

                _specialOffersRepository.Save();
            }

            Status.AppendLine(string.Format("<p>Special Offers Added: {0}</p>", specialOffersAdded));
        }

        #endregion

        #region Venues

        public void UpdateVenues()
        {
            var xmlVenues = _xmlParser.GetVenues();
            var venuesAdded = 0;

            foreach (var xmlVenue in xmlVenues)
            {
                if (ProcessErrors(xmlVenue))
                    continue;

                var venue = _venuesRepository.FirstOrDefault(v => v.MerchantVenueId == xmlVenue.MerchantVenueId);

                if (venue == null)
                {
                    venue = AddVenue(xmlVenue);
                    _venuesRepository.Insert(venue);
                    venuesAdded++;
                }
                else
                {
                    venue.LocationCode = string.IsNullOrEmpty(xmlVenue.LocationId) ? venue.LocationCode : xmlVenue.LocationId;
                    venue.Name = string.IsNullOrEmpty(xmlVenue.Name) ? venue.Name : xmlVenue.Name;
                    venue.Address1 = string.IsNullOrEmpty(xmlVenue.Address1) ? venue.Address1 : xmlVenue.Address1;
                    venue.Address2 = string.IsNullOrEmpty(xmlVenue.Address2) ? venue.Address2 : xmlVenue.Address2;
                    venue.Postcode = string.IsNullOrEmpty(xmlVenue.Postcode) ? venue.Postcode : xmlVenue.Postcode;

                    if (venue.Postcode != null && venue.Postcode.Length > 10)
                        venue.Postcode = null;

                    if (!venue.Longitude.HasValue && !venue.Latitude.HasValue)
                    {
                        venue.Longitude = xmlVenue.Longitude;
                        venue.Latitude = xmlVenue.Latitude;
                    }

                    venue.Directions = string.IsNullOrEmpty(xmlVenue.Directions) ? venue.Directions : xmlVenue.Directions;
                    venue.NearestTube = string.IsNullOrEmpty(xmlVenue.NearestTube) ? venue.NearestTube : xmlVenue.NearestTube;
                    venue.TubeDirection = string.IsNullOrEmpty(xmlVenue.TubeDirection) ? venue.TubeDirection : xmlVenue.TubeDirection;
                    venue.Underground = string.IsNullOrEmpty(xmlVenue.Underground) ? venue.Underground : xmlVenue.Underground;
                    venue.RailwayStation = string.IsNullOrEmpty(xmlVenue.RailwayStation) ? venue.RailwayStation : xmlVenue.RailwayStation;
                    venue.BusRoutes = string.IsNullOrEmpty(xmlVenue.BusRoutes) ? venue.BusRoutes : xmlVenue.BusRoutes;
                    venue.NightBusRoutes = string.IsNullOrEmpty(xmlVenue.NightBusRoutes) ? venue.NightBusRoutes : xmlVenue.NightBusRoutes;
                    venue.CarPark = string.IsNullOrEmpty(xmlVenue.CarPark) ? venue.CarPark : xmlVenue.CarPark;
                    venue.InCongestionZone = string.IsNullOrEmpty(xmlVenue.InCongestionZone) ? venue.InCongestionZone : xmlVenue.InCongestionZone;
                    UpdateVenueFacilities(xmlVenue, venue);
                    UpdateVenueImages(xmlVenue, venue);
                }
            }

            _venuesRepository.Save();

            Status.AppendLine(string.Format("<p>Venues Added: {0}</p>", venuesAdded));
        }

        private Venue AddVenue(XmlVenue xmlVenue)
        {
            var venue = new Venue
            {
                MerchantVenueId = xmlVenue.MerchantVenueId,
                LocationCode = xmlVenue.LocationId,
                Slug = CommonFunctions.GetFriendlyUrl(xmlVenue.Name),
                Name = xmlVenue.Name,
                Address1 = xmlVenue.Address1,
                Address2 = xmlVenue.Address2,
                Postcode = xmlVenue.Postcode,
                Latitude = xmlVenue.Latitude,
                Longitude = xmlVenue.Longitude,
                Directions = xmlVenue.Directions,
                NearestTube = xmlVenue.NearestTube,
                TubeDirection = xmlVenue.TubeDirection,
                Underground = xmlVenue.Underground,
                RailwayStation = xmlVenue.RailwayStation,
                BusRoutes = xmlVenue.BusRoutes,
                NightBusRoutes = xmlVenue.NightBusRoutes,
                CarPark = xmlVenue.CarPark,
                InCongestionZone = xmlVenue.InCongestionZone,
                ShowOnMap = false,
                Active = true
            };

            if (venue.Postcode != null && venue.Postcode.Length > 10)
                venue.Postcode = null;

            AddVenueImages(xmlVenue, venue);
            AddVenueFacilities(xmlVenue.XmlFacilities, venue);

            return venue;
        }

        private void AddVenueFacilities(IEnumerable<string> facilities, Venue venue)
        {
            foreach (var facilityToAdd in facilities)
            {
                var facility = _facilitiesRepository.FirstOrDefault(f => f.Description == facilityToAdd) ?? AddFacility(facilityToAdd);

                if (facility != null && facility.Id > 0)
                    venue.Facilities.Add(facility);
            }
        }

        private static void AddVenueImages(XmlVenue xmlVenue, Venue venue)
        {
            foreach (var xmlImage in xmlVenue.XmlImages)
            {
                var venueImage = new VenueImage
                {
                    ImageTypeId = (byte)xmlImage.ImageType,
                    Filename = xmlImage.ImageUrl,
                    Modified = true
                };

                venue.VenueImages.Add(venueImage);
            }
        }

        private void UpdateVenueFacilities(XmlVenue xmlVenue, Venue venue)
        {
            var facilitiesToDelete = venue.Facilities
                .Where(fd => !xmlVenue.XmlFacilities.Contains(fd.Description)).ToList();

            var facilitiesToAdd = xmlVenue.XmlFacilities
                .Where(fa => !venue.Facilities.Select(vf => vf.Description).ToList().Contains(fa));

            facilitiesToDelete.ForEach(fd => venue.Facilities.Remove(fd));

            AddVenueFacilities(facilitiesToAdd, venue);
        }

        private static void UpdateVenueImages(XmlVenue xmlVenue, Venue venue)
        {
            var venueImageFileNames = GetImageFileNames(venue.VenueImages);

            foreach (var xmlImage in xmlVenue.XmlImages)
            {
                var fileName = CommonFunctions.GetFileName(xmlImage.ImageUrl);

                if (!venueImageFileNames.Contains(fileName))
                {
                    venueImageFileNames.Add(fileName);

                    var venueImage = new VenueImage
                    {
                        ImageTypeId = (byte)xmlImage.ImageType,
                        Filename = xmlImage.ImageUrl,
                        Modified = true
                    };

                    venue.VenueImages.Add(venueImage);
                }
            }
        }

        #endregion

        #region Helpers

        private static byte EnsureUniqueCoverImage(ImageTypes imageType, Show show)
        {
            if (imageType == ImageTypes.Cover)
            {
                if (show.ShowImages.Any(si => si.ImageTypeId == (byte)ImageTypes.Cover))
                    return (byte)ImageTypes.Standard;

                return (byte)ImageTypes.Cover;
            }

            return (byte)imageType;
        }

        private static List<string> GetImageFileNames(IEnumerable<Media> media)
        {
            var imageFileNames = new List<string>();

            foreach (var image in media.Where(m => m.Filename.StartsWith("http")))
            {
                var fileName = CommonFunctions.GetFileName(image.Filename);

                if (!string.IsNullOrEmpty(fileName))
                    imageFileNames.Add(fileName);
            }

            return imageFileNames;
        }

        private bool ProcessErrors(IXmlError entity)
        {
            if (entity.XmlErrors.Any())
            {
                foreach (var xmlError in entity.XmlErrors)
                    Status.AppendLine(string.Format("<p>{0}</p>", xmlError.Description));

                if (entity.XmlErrors.Any(xe => xe.XmlErrorType == XmlErrorType.Error))
                    return true;
            }

            return false;
        }

        #endregion
    }
}
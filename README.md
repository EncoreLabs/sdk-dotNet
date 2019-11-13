# EncoreTickets SDK

This repository contains the main project EncoreTicket.SDK and other projects for testing classes from the main.

This SDK contains .Net models and service classes for calling Encore Tickets API, as well as general useful utilities and interfaces.

Each service has public methods that wrap a call to a specific API endpoint with some parameters.

#### The following service classes are available:

- InventoryServiceApi.cs - For calling endpoints of Inventory APP (proxy searches from the Elastic service & Thebs)

- ContentServiceApi.cs - For calling endpoints of Content APP (rich content for products)

- VenueServiceApi.cs - For calling endpoints of Venue App (the venue service provides information on product locations)

- BasketServiceApi.cs - For calling endpoints of Basket API service

- CheckoutServiceApi.cs - For calling endpoints of Checkout API (for Encore Tickets Ltd)

- PricingServiceApi.cs - For calling endpoints of Pricing API (live pricing for products available for sale through Encore Tickets)

### Instalation

EncoreTickets.SDK is available as a Nuget package. You can use the Nuget Package Manager to install it.

```sh
Install-Package EncoreTickets.SDK
```

### Versions:

#### 1.5
- Updated methods to return an exception instead of a default value if API request fails

#### 1.4
- Added a method to the BasketServiceApi to call GET /api/v1/baskets/{reference} endpoint

#### 1.3
- Added a method to the PricingServiceApi to call GET /api/v2/admin/exchange_rates endpoint

#### 1.2
- Added a method to the VenueServiceApi to call POST /api/v1/admin/venues/{id} endpoint
- Added a mapping between environment names and Environments enum

#### 1.1
- Added a method to the BasketServiceApi to call GET /api/v1/promotions/{id} endpoint

# EncoreTickets SDK

This repository contains the main project EncoreTicket.SDK and other projects for testing classes from the main.

This SDK contains .Net models and service classes for calling Encore Tickets API and working with some AWS services as well as general useful utilities and interfaces.

Each service has public methods that wrap a call to a specific API endpoint with some parameters.

#### The following service classes are available:

- InventoryServiceApi.cs - For calling endpoints of Inventory APP (proxy searches from the Elastic service & Thebs)

- ContentServiceApi.cs - For calling endpoints of Content APP (rich content for products)

- VenueServiceApi.cs - For calling endpoints of Venue App (the venue service provides information on product locations)

- BasketServiceApi.cs - For calling endpoints of Basket API service

- CheckoutServiceApi.cs - For calling endpoints of Checkout API (for Encore Tickets Ltd)

- PricingServiceApi.cs - For calling endpoints of Pricing API (live pricing for products available for sale through Encore Tickets)

- AwsSqs.cs - For working with AWS Simple Queue Service

### Instalation

EncoreTickets.SDK is available as a Nuget package. You can use the Nuget Package Manager to install it.

```sh
Install-Package EncoreTickets.SDK
```

### Versions:

#### 3.0
- All exceptions related to the API calls are now inheriting from ApiException
- Removed obsolete classes and functionality (including Entertain API)
- Refactored multiple classes which resulted in namespace changes as well as splitting of some classes
- Added multiple helper methods for common operations with API models
- Improved request and response JSON serialization and deserialization
- Added cache handling logic

#### 2.4
- Added a method to the BasketServiceApi to call PATCH /api/v1/baskets endpoint
- Added a method to the BasketServiceApi to call DELETE /api/v1/baskets/{reference}/reservations/{reservationId} endpoint
- Added a method to the BasketServiceApi to call PATCH /api/v1/baskets/{reference}/clear endpoint

#### 2.3
- Added ability to send messages to AWS queue

#### 2.2
- Added public interfaces for API services
- Added an ability to create an authentication service for some API with a specific context

#### 2.1
- Added a method to the BasketServiceApi to call PATCH ​/api​/v1​/baskets​/{reference}​/applyPromotion endpoint

#### 2.0
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

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

## Instalation

EncoreTicket.SDK is available as a Nuget package. You can use the Nuget Package Manager to install it.

```sh
Install-Package EncoreTicket.SDK
```

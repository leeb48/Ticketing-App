# Ticketing App

## Overview
- Be able to view list of events that are available
- Events detailed page to show available tickets and a seatig map
- Be able to book the event

## Notes
User:
- booking []

Ticket:
- event
- price
- status (available/sold)
- seat

Booking:
- user
- ticket []
- totalCost
- status (processing/booked)

Event:
- name
- description
- date
- type
- venue
- artist

Artist:
- name
- description
- event []

Venue:
- name
- address
- seat []
- event []

Seat:
- venue
- section
- row
- col

# Start here

## AWS TODO
- [ ] learn Pulumi to deploy the application on AWS
- [ ] host using multiple containers in distributed mode
- [x] dockerize the application and setup github actions to push the docker image

## Application TODO
- [x] postgres full text search for artist
- [x] setup search input to execute search and display the results as a list using htmx
- [x] display list of artists with pagination
- [x] edit page for artists

- [x] Fix DB Migration issue
- [x] Use the pagination service with venue entity
- [x] CRUD Venue and seats
- [x] create a seat map for the venue
- [x] scaffold the main page with search bar, event list page, and event details page
- [x] refactor pagination (maybe create a class for this)
- [ ] refactor controller to be lean and create services to handel business logic

## Bug
- [x] full text search with spaces are not working

## Checkout
- [ ] create checkout step using stripe
 - [x] after/during checkout, booking will be in processing. After transaction is processed, then update the status to booked
 - [ ] redirect the user to a order confirmation page (show order status, ticket info, event info)
- [ ] prevent going back to checkout page after purchasing

## Authentication
- [ ] Create an auth controller to handle user session using JWT
- [ ] do not show edit button when doing search on the main page
 - [ ] check the authentication status in the event controller and pass in the data to the pagination model

## Booking
- [ ] create a user session so that multiple booking objects are not created when checkout is clicked multiple times

## Alerting
- [ ] rework alerts so that it can be shown across different pages

## Tickets
- [ ] add prices to tickets
- [ ] display the correct price amounts on the checkout and reserve pages
- [ ] free up tickets after the event (single and batch)

## Seat reservation
- [x] main event search bar that lists the events
- [x] send the selected seats to booking controller and retrieve the correct tickets
- [x] create the booking and display it on the checkout page
- [x] mark seats on the venue based on their status (available, sold, pending)
 - [x] use distributed lock to mark the seats during checkout step

## Events
- [x] create the tickets when events are created
- [x] Create/update events and link artist and venue.
 - [x] During event update, display the previously selected artist and veneue.
 - [x] When creating events, we can perform search to pick the artist and venue.
- [x] Display artist info on the events page.
- [x] Display the seat map on the events page.

## Backlog
- [ ] Allow adding spacers and empty seats during create/update venue

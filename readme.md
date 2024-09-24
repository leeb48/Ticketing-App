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


## Venue & Seat
- [ ] mark seats on the venue based on their status (available, sold, pending)

## Events
- [ ] Create/update events and link artist and venue.
 - [ ] During event update, display the previously selected artist and veneue.
 - [x] When creating events, we can perform search to pick the artist and venue.
- [ ] Display artist info on the events page.
- [ ] Display the seat map on the events page.

## Backlog
- [ ] Allow adding spacers and empty seats during create/update venue

## AWS TODO
- [x] dockerize the application and setup github actions to push the docker image
- [ ] setup ECS to host the application
- [ ] setup docker actions to deploy the docker image onto ECS cluster
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
- [ ] CRUD Venue and seats
- [ ] create a seat map for the venue
- [ ] mark seats on the venue based on their status (available, sold, pending)

- [ ] scaffold the main page with search bar, event list page, and event details page
- [ ] refactor pagination (maybe create a class for this)

## AWS TODO
- [x] dockerize the application and setup github actions to push the docker image
- [ ] setup ECS to host the application
- [ ] setup docker actions to deploy the docker image onto ECS cluster
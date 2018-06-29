# Software Requirements
## Vision
What is the vision of this product?
Playlist Mixer provides various music playlists.

What pain point does this project solve?
We want to provide entertainment for the community. This is a free of charge service.

Why should we care about your product?
We want to provide the highest quality of playlists based user preferences.

Scope (In/Out)

        IN
        Provide playlists of songs
        Display list of songs in the selected playlist
        Allow users to search by genre and time period
        
        
        OUT
        Does not play music directly

## MVP
- display a playlist based off of user searches by genre and/or time period
- songs with title, artist, album, and time period displayed
- Allow user to create an account (user name only, no password)
- Allow existing users to log back in

Stretch
- Responsive design
- Allow a user to view other users
- Pre-generated playlists saved to custom API
- Display album artwork and artist description
- Let users create and edit custom playlists
- Allow a user to add new songs to database
- Allow a user read access to another user's playlist

Functional Requirements

List the functionality of your product. This will consist of tasks such as the following:

    We want to be able to control our database/API query so that we can generate customized playlists.
    We want to connect with the database so that we can a user name and their playlists.
    A user can search all of the songs in the database.

## Non-Functional Requirements

### Availability
This web application will be hosted on Azure Web Services.

### Security
The security of this application will be non-existent. Therefore, users will be warned not to make an account that is linked to personal identifiable information.

### Testability
This application shall include unit tests with a total code coverage of no less than 90%.

### Usability
Any user will have access to our application. Any user should be able to create playlists.

Since our application will be hosted on Azure Web Services, the availability of our application will dependent on the stability of Azure's servers. Because users will have access to our application without a password, testing for security will not be necessary.

### Flow of Data
When a user logs in (without password), the user will be saved onto the database (if it doesn't already exist). When a user sends a request, a query is sent to our API that has pre-built playlists retrieved from a third party API. Matching playlists are then sent from our API back to the user on the web application.

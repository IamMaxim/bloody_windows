using System.Collections.Generic;

namespace API
{
    public class Person
    {
        public string username;
        public string email;
        public string password;

        public Person(string username, string email, string password)
        {
            this.username = username;
            this.email = email;
            this.password = password;
        }
    }

    public class User
    {
        public string email;
        public string username;
        public string id;

        public User(string email, string username, string id)
        {
            this.email = email;
            this.username = username;
            this.id = id;
        }
    }

    public class Play
    {
        public string id;
        public string date;
        public string score;
        public string mapid;
        public string user;

        public Play(string id, string date, string score, string mapid, string user)
        {
            this.id = id;
            this.date = date;
            this.score = score;
            this.mapid = mapid;
            this.user = user;
        }
    }
    
    public class Content
    {
        public string location;
        public string description;
        public string profile_picture_url;

        public Content(string location, string description, string profile_pictur_url)
        {
            this.location = location;
            this.description = description;
            this.profile_picture_url = profile_pictur_url;
        }
    }

    public class Profile
    {
        public string id;
        public string user;
        public string description;
        public string location;
        public string date_joined;
        public string hours_played;
        public string successful_map_plays;
        public string map_plays;
        public string total_score;
        public string profile_picture_url;
        public List<string> friends;

        public Profile(string id, string user, string description, string location,
            string dateJoined, string hoursPlayed, string successfulMapPlays,
            string mapPlays, string totalScore, string profilePictureUrl, 
            List<string> friends)
        {
            this.id = id;
            this.user = user;
            this.description = description;
            this.location = location;
            this.date_joined = dateJoined;
            this.hours_played = hoursPlayed;
            this.successful_map_plays = successfulMapPlays;
            this.map_plays = mapPlays;
            this.total_score = totalScore;
            this.profile_picture_url = profilePictureUrl;
            this.friends = friends;
        }
    }

    public class Token
    {
        public string refresh;
        public string access;

        public Token(string refresh, string access)
        {
            this.refresh = refresh;
            this.access = access;
        }
    }
}
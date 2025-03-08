# Introduction
 
Mimir Candidate,

Welcome to the technical interview for our project. The following ten open-ended questions are designed to evaluate your technical proficiency, critical thinking, reasoning, analytical abilities, and communication skills. We expect you to provide thorough and thoughtful responses that reflect your unique approach and expertise.

# Hands-On Coding Exercises: GitHub Project Upgrade

**Instructions:**

1. **Clone the Repository:** 

   Begin by cloning the project repository from GitHub. This will be your working codebase for these exercises.

2. **Create a New Readme.md**

   Describe your proficiency with the technologies used and/or referenced within this repository and these exercises.

   I have experience working with MVC design in my Orases internship. I created a small functioning website connected to a database in the few weeks I was there.
   I used PHPStorm, Cake, docker, and other tools to create and maintain the website and set up a chron job to monitor the performance.

   I have experience using API's from college. I created a Magic the Gathering card search program that would return card images when searched. Unforunately
   the program has been lost but the design was simple because I created it over a day or two. I found an API that I could use to request card information and
   after reading their documentation it was very simple to navigate. The data was stored in JSON format and it was simple to turn that into a card object with
   a matching data template and then take out the pieces I needed and draw them to the screen.

   I also have experience taking data from flight simulators and updating user interfaces in real time to reflect the machines state or display flight training information.


   

3. **Make Your Changes:** 

   Select a minimum of four exercises from the below list for this interview process. For each of the exercises that you select, make the necessary modifications directly in the code. As you work, document your changes by including clear explanations (within your new readme.md file) that describe **how** you implemented your solution and **why** you chose that particular approach. 

4. **Submit Your Solution:** 

   After completing all the exercises, upload the updated private repository to your personal GitHub account. Then, email us the private repository link along with your explanations. Your submission will be reviewed for coding efficiency, technical proficiency, and the clarity of your documentation.

Good luck!

---

## 1. Solution Architect Experience

Clone the project and review its overall architecture. Implement changes to enhance scalability—such as integrating a caching layer or restructuring into a multi-tier architecture. Document your approach, explaining your design decisions and how these changes improve scalability and maintainability.
 
 Designing a caching layer would depend ulitimately on the user traffic. In the case of weather data it might be pracitcal to use location data to pre load weather data for the area
 the user is accessing from. There are also global trends to consider such as natural disasters that will lead to a lot of traffic to that data, implementing a "hotspot" feature to
 cache data related to trends might be a valubale asset in times of high user traffic. To expand the application to be used for multiple customers or projects it would be important
 to create each unit such as the database, user interface, and control units seperately. The flight simulators I worked on were built in this way that there was a pool of data
 that could be accessed through a shared data set but each unit was devloped and updated independantly following a standard so any updated or changes could be made and reused
 across the entire fleet from each department. An example would be creating database interface code that follows the standard used across all jobs with expected values and queries
 but each project in the repo can J10024 for example will have queries related to weather data and J6203 will have Home Goods or Accounting Data. The user interface may not need
 any adjustments to interact with the code just with the output to the user so this helps keep hands out of code that doesn't need to be updated or is being handled by another team.
 The most important part of this approach is that every team follows a standard set ahead of time and has meetings to make sure that any adjustment is confirmed by any two departments
 for data types, data behavior, or any other change that may need to be implemented. Scalability vertically would come down to compartmentilization of data and effective queries/
 database structure. Having to search all weather data across the whole world might not be very efficent if were only looking by city. Offering the user a dropdown box that has them
 select from a continent, country, or region might be appropriate. This would allow the site to run a script to load all data for that country first and then begin to search by city.
 The dropdown would limit user error when searching and ensure that the database always returned valid data as well as offer helpful tips if a user didn't know how to spell a city
 name. Another approach is to have users log in, select a location, or use their location if allowed to pre emptively limit the data searched. This also ties back to the caching
 technique described earlier.

---

## 2. Database Implementation

Examine the current database implementation and upgrade the data-access layer. Consider refactoring it to better utilize an ORM or improve the existing schema design. Modify the code accordingly and include a detailed explanation of your changes and the reasoning behind them.

---

## 3. Database Performance for Postgres

Identify one or more queries interacting with the Postgres database that could be optimized. Enhance their performance by refactoring the query logic, adding indexes, or implementing partitioning strategies. Update the project, and provide documentation on the performance improvements and your approach.

---

## 4. Design Patterns

Select a section of the code (such as controllers, services, or repositories) that could benefit from a specific design pattern (e.g., Repository, Factory, or Strategy). Refactor the code to incorporate this design pattern. Explain your choice, how you implemented it, and the benefits it brings to the project.

---

## 5. C#.NET Core Enhancements

Review the backend C#.NET Core code and implement enhancements to improve performance and maintainability. This might include leveraging asynchronous programming, custom middleware, or newer language features. Modify the code and document your implementation process and the rationale behind your decisions.

---

## 6. MVC Dependency Injection

Analyze the current usage of dependency injection within the MVC framework in this project. Identify areas where DI can be better applied, refactor the code to enforce proper DI practices, and provide a summary of your modifications and the benefits achieved through better decoupling and testability.

---

## 7. HTML/CSS Mixed UX Experience

Evaluate the user interface built with HTML/CSS and implement enhancements to improve responsiveness and overall user experience. Update the UI for better accessibility and visual consistency across devices. Document your changes and explain your design choices.

The user interface should be tailored to the experience that we expect the user to have as well as the target demographic. For this existing site there should be data to track
the most viewed pages. Those pages should be added to the home page or even the relevant data added to the home page if applicable to create an environment where users can very
quickly access the data they want. Every page needs a consistent way to access the site toolbar so a user can search, go home, find upcoming storms, etc. There should never be a 
"dead end" where the user loses access to any other part of the site they may need to reach. The toolbar needs labeling that is informative and consice as to not overload the user
with icons or text while still presenting them with what they need.

---

## 8. OOP C# Advanced Reasoning Skills

Identify one or more classes where object-oriented principles (like encapsulation, polymorphism, or adherence to SOLID principles) could be improved. Refactor these classes to enhance their design and functionality. Include an explanation of the refactoring process and the benefits of your approach.

All the code in Index.razor, MainLayout.razor.css, and any other hard coded descriptions need to be refactored to take variables. These variables need to be stored in an approved
list that is updated by the project administrator so there is never an accident and font or colors unapproved by managment end up on the site. But having hard coded values is almost
always a no go and even if the design is going to remain the same 99% of the time it makes the workload for changing it that much easier. Imagine even something fun for the user
like updating a "snow theme" during the holidays with different fonts and colors. This could be created as a "snow theme class" swap out the variables with ones for the theme and
have the page call the updated snow theme rather than making any changes to the standard files. The second and more important case is developing a mobile and desktop application and
being able to reuse any code. Line 31 in Index.razor "width: 550px;" is not always going to fly on different devices. I have never developed a mobile app but I would imagine using
even something as ""width: (screenSize/4)" is already a much better solution to keep the page size consistent across desktop or mobile displays. At minimum this will help the
desktop application run smoothly on a tablet assuming the code base won't transfer one to one from desktop to mobile.

---

## 9. JavaScript Enhancements

Review the client-side JavaScript code and add an interactive feature or refactor existing functionality to improve modularity and maintainability. Make the necessary code changes and provide documentation on how and why your enhancements improve the overall project.

An exciting feature that could be added is a simple pop-up message that updates the user whenever there is an inclimate weather update or extreme weather update in their area. 
Assuming the previous location update was implemented for users that log in they could turn on live weather reports or updates on inclimate weather while browsing (this is the
little brother of live mobile app updates). Weather updates should be running on a chron job to keep each region/city/country etc up to date. Each time the table is updated with a
new weather report the data could be searched for a tag called "Extreme" if the value is true it could grab that entry and the relevant data (city, temprature, tornatdo warning, 
etc.) and write a message to the user. The template could be reused for every location:

City + "has a severe weather update! Click to learn more."

Then once the message is created you create an alert box with the message in it and an option to go to the city page or to close.

if (confirm("Go to city page?")){

goToCityPage(City)

}

else{close window}

The "Extreme" that this message is predicated on would depend on how the data is gathered and sent to the database. Raw data would need to be interpreted as something meaningful
like if windspeed > 40 set extreme to true (for a tornado warning). If temprature > 100 set extreme to true (assuming using degrees F for a heat wave). I don't have the data 
classifications that qualify weather to be an extreme weather report but I imagine there is a standard that can easily be adopted to create this value.

The reason this type of message would be valuable is it would keep users engaged with the site while they were not only browsing but may keep them browsing for longer as they
wait for live weather updates. Asking them to go to the page is deliberate as well. The idea is to create something exciting for the user and have them click to find out, this 
can create engagement and website traffic that otherwise wouldn't exist if the message provided more information. It also allows a more robust description of the extreme weather
patterns on the City page rather than trying to express the informaion in a small alert box.

---

## 10. RESTful APIs and JSON

Examine the current RESTful API endpoints and implement improvements to enhance efficiency, security, or functionality. For example, update an endpoint to return JSON data with improved validation and error handling. Modify the project accordingly and document your approach and rationale.

I was looking over the JSON structure in the weatherCache.json file and it was unreadable from being on one line but I imagine that was an error with the file being created
or something. So I formatted it and I feel like it didn't make it any easier to read. The beauty of JSON format was you could have something like Noida or El Dorado Hills and 
they could have nice neat attributes like temprature, feels like, humidity, etc. and that data can be parsed and used any way you want while being easy to read. I don't have the 
documentation so I don't know how to use the u0022 codes but that makes the json hard to read. Also on line 213 on index.razor there is a call using ipwhois that just grabs user
location data. I'm no security expert but I don't think it should be just hanging out in the code unprotected like that. There should probably be a flag that says if the user allows
the location data to be used in order to keep that information protected if the user doesn't want their location being used. Also the temp data that it returns is unlabeled. There 
is no way to tell if it is in degrees F or C. It should probably offer both if possible or if not there needs to be a conversion process on the application end to store both
values and offer the user an option to chose one or even display both.

## 11. Ending notes


I'm sorry I wasn't able to get the project to run. I was working for a couple hours on fixing dll issues, framework issues, sdk issues. I'm no good with all the compiler stuff
so I wasn't able to get it running. I have to work until tuesday 12 hour shifts so I won't be able to work on this any more until after then which I can pretty confidently say
other canidates would have submitted something so I wanted to give my best shot at answering the questions and discussing the design approach. I have very strong skills in design
concepts and cultivating a user experience that I wanted to try and express. Anyone can google how to implement a clickable button or a binary search tree to make data load fast, I 
think true programming starts with being able to conceptualize the problems and design them out ahead of time. I hope I will get a chance to talk with you further and I really 
appreciate your time and consideration for the position.

Trey Castrechino

---

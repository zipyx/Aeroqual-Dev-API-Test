# Aeroqual Cloud Developer Test

This test is to create a simple API that serves data from a json file.

The JSON file is data.json, and contains a list of people, with their names and ages.

We would like you to create a RESTful API that allows you to create, update, delete, and get people.

In addition, we would like to be able to search by a person's name. The input may not always be a complete name, e.g. if the input is "hat", we expect "hat", "hatter", "that" etc to be returned.

You are free to layout the project however you wish. 

We would like to see what you would consider production quality code.

Please commit your code to a private github repository and share it with maxgruebneraeroqual.

If you cannot do that for some reason, please zip the source code and email it to the recruiter.

If you have made any design decisions that you would like to explain please add them to this file.


--------------------------------------------------------------------------------------------------------------------------

# MVC Repository Pattern with Library
I've designed the structure of the project to include an API library with a repository pattern for abstraction between data and the business logic coupled with MVC on an application layer. This is also a little different as I've placed the repository pattern within a library only, reason being is ideally I would have used something like Entity Framework to migrate JSON objects into a database but I wasn't sure what the constraints were for this test since it was specified that data must be served FROM a json file.

# Dbcontext (Incomplete)
You'll also see that the respository code within the library in relation to it's "context" is incomplete, it cannot be complete as I don't have a database
to work with but thought it's good practice to conceptually display the idea, that I would migrated the json objects into a relational database, creating it's relationships with other inbuilt frameworks.

# Security (Incomplete)
I've added security using JWT instead of Auth0 for API's authorization, this is also incomplete on purpose as this is only a test but thought it was a worthwhile to mention here as it is important to secure API access for production ready use. I decided for simplistic sake, it might be better to use JWT as it provides manageable access over.

# Unit Testing
I've also briefly included some basic unit testing to disply that production code should always be tested and staged before release to production, they should have some form of integration testing as well as unit and behavioural to ensure that data in motion or at rest isn't exploited.

# Other Reasons
Other reasons for creating a library for this particular test is to show that standard libraries are portable and work well with many projects, there are no dependencies on the particular application but an easy to access library simplying the complexity of a project, I'm demonstrating this by including an AWS lambda & AWS API Gateway, that is included in the solutions file.

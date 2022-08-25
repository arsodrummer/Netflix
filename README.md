# Messaging features based on NServiceBus + WEB API (.NET Core)
![image](https://user-images.githubusercontent.com/16143411/184549524-91da71bb-1c37-4860-a918-1a494a5e7934.png)

**Intro**

This project demonstrates how we can use subscriptions + email notifications based on messaging technology from [NServiceBus](https://particular.net/nservicebus).

The main idea of this project is to show how to use Sagas and Timeouts closely to the real life subscription service - Netflix. Of course you can reuse for your own ideas, business or pet projects.

We have a simple API that allows us to manipulate with Users, Plans and Subscriptions by using [Postman](https://www.postman.com/).

The database should be deployed in MS SQL.

As a SMTP server localy you should use [smtp4dev](https://github.com/rnwood/smtp4dev) tool.

**Flow**

![Netflix_flow](https://user-images.githubusercontent.com/16143411/184550959-2132f539-2adc-434b-8f66-b3fffa60699e.png)


**DbUp**

After you've set up your database localy you should create some useful tables there. Set the DbUp project as the Startup project, then build it and run. 

You'll see in the output window that all scripts have been executed successfuly.

After that you should get some new tables like:

![image](https://user-images.githubusercontent.com/16143411/184550231-fa0321e1-710a-4bcd-a245-8323e50661cc.png)

Then please set both projects Api and NServiceBus as the Startup projects and run solution.

**Postman**

Use it for data manipulating and for sending notifications. After you've installed Postman, import [postman collection](https://github.com/arsodrummer/Netflix/blob/master/Postman/Netflix.postman_collection.json) there.

You should get something like this:

![image](https://user-images.githubusercontent.com/16143411/184550296-0c4a9f2b-4550-48f5-b908-df506ca3a669.png)

That's it. Enjoy and feel free to share your thouts about it.

# Twitter Application
A modern Twitter application built with **Clean Architecture**, utilizing the **CQRS** and **Mediator** patterns, and developed using .NET 6, Angular, MongoDB, Redis, and various other technologies.
## Description
The Twitter Application is a powerful social media platform designed to provide users with a seamless and intuitive experience. It follows the principles of **Clean Architecture** to ensure maintainability, testability, and scalability. The application leverages the **CQRS** (Command Query Responsibility Segregation) pattern and **Mediato**r pattern to separate commands and queries, leading to improved performance and code organization.

## Key Features
* **User Registration and Authentication:** Users can create an account, log in, and secure their profile with authentication mechanisms.

* **Tweeting and Interacting:** Users can compose and post tweets, like and comment on tweets, and engage in meaningful conversations.

* **Following and Followers:** Users can follow other users to receive their tweets in their timeline and explore a network of followers.

* **Search and Discovery:** Users can search for other users by username, enabling them to connect with new people and discover interesting content.

* **Real-time Updates:** The application provides real-time updates using technologies like WebSockets, ensuring users receive instant notifications and live interactions.

## Technologies and Architecture
The Twitter Application is built using the following technologies and architectural principles:

* **Clean Architecture:** The codebase follows a clean architecture pattern, separating concerns into layers such as domain, application, and infrastructure, promoting maintainability and testability.
* **Backend**: .NET 6, ASP.NET Core, MongoDB for data storage, Redis for token storage, MediatR for implementing the Mediator pattern, AutoMapper for object-object mapping, Serilog for logging, FluentValidation for input validation, and XUnit for unit testing.

* **Frontend:** Angular framework for building the user interface, utilizing TypeScript, HTML, and CSS.

* **Additional Services:** Mailkit for email communication (account confirmation, password reset), Cloudinary for photo upload and storage.

## Contact
If you have any questions or feedback regarding the Twitter Application, feel free to reach out:

**Email**: asifabdullah135@gmail.com, raihannishat.swe@gmail.com

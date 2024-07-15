# RepoRanger
[![Build](https://github.com/James-o-Howson/RepoRanger/actions/workflows/build.yml/badge.svg)](https://github.com/James-o-Howson/RepoRanger/actions/workflows/build.yml)

RepoRanger is a comprehensive tool designed to clone repositories, parse them, and maintain a detailed list of their
projects and dependencies. With RepoRanger, you can easily manage and monitor your project dependencies, ensuring your
codebase remains secure and up-to-date.

## Key Features
- Repository Cloning: Automated cloning of repositories to keep track of various projects and their dependencies. 
- Dependency Management: Automatically parse repositories to generate a comprehensive list of project dependencies.
- Vulnerability Detection: Utilize data from [osv.dev](https://osv.dev/) to identify and report dependency vulnerabilities.
- User Interface: Access an intuitive UI to browse through project dependencies and vulnerabilities.
- Notifications: Opt-in to receive alerts when new vulnerabilities are detected in your dependencies.

RepoRanger helps you maintain a secure and well-managed codebase by providing critical insights into your project's
dependencies and their vulnerabilities.

## Overview
Angular front end and .net api are built and tested using GitHub Actions upon being pushed to the develop and master
branches.

### repo-ranger-app
Angular UI that provides a method of visualising and searching project dependencies and vulnerabilities. Uses PrimeNG
as the Component Library.

### RepoRanger.Api
RESTful Api that provides access to Repository, Project and Dependency data. Also runs Quartz services that
clones and parses repositories for dependency information and also queries osv.dev for dependency vulnerability data.

Key patterns and architectural decisions:
- CQRS
- DDD
- Clean Architecture

Key dependencies:
- Mediatr
- Entity Framework
- Quartz
- Fluent Validation

The following pre-build steps are defined:
- Generate [osv.dev](https://osv.dev/) C# Client

The following post-build steps are defined:
- Generate swagger.json openapi specification
- Generate Angular Typescript Client
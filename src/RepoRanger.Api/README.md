# Overview
Angular front end and .net api are built and tested using GitHub Actions upon being pushed to the develop and master
branches.

## repo-ranger-app
Angular UI that provides a method of visualising and searching project dependencies and vulnerabilities. Uses PrimeNG
as the Component Library.

## RepoRanger.Api
RESTful Api that provides access to Repository, Project and Dependency data. Also runs Quartz services that
clones and parses repositories and also queries osv.dev for dependency vulnerability data.

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
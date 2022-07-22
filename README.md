# ZeroPass Server

ZeroPass is more than a Password Protection and Management tool, it secures all your valuable digital assets in your own vault, including the personal vault and the company vault

## Getting Started

This project is the server part of ZeroPass, it needs to work with the [client part](https://github.com/metaguardpte/ZeroPass-Client) on a live system. 

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

```
. Visual Studio 2019/2022 with .Net Core 3.1
. Redis
. Mysql
```

### Installing

```
1. Open the ZeroPass.sln file by Visual Studio 2019/2022
2. Modify the launchSettings.json file to update your configuration:
    2.1 "MysqlConnectionString": Read/write database connection string of the mysql
    2.2 "ReadonlyMysqlConnectionString": Read only database connection string of the mysql
    2.3 "RedisConnectionString": Read/write connection string of the redis
    2.4 "ReadonlyRedisConnectionString ": Read only connection string of the redis
    2.5 "SMTP_SERVER": Smtp server name/ip address
    2.6 "SMTP_USER": Smtp login user name
    2.7 "SMTP_PASSWORD": Smtp login password
    2.8 "JWT_SECURITY_KEY": Security key for jwt creation/authorization
    2.9 "JWT_EXPIRES_SECONDS": Token expiration seconds
3. Run ZeroPass.Api project
```
## Running the tests

```
Open Test Explorer and run the Tests by Visual Studio 2019/2022
```

## Deployment

Windows:
```
dotnet ZeroPass.Api.exe
```
Linux:
```
dotnet ZeroPass.Api.dll
```
Docker:
You are free to package the backend as a Docker image and run it
```
1. docker build -t zeropass.api . 
2. docker run -it --rm -p 5001:5001 -- zeropass_sample zeropass.api
```
## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) and [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md) for details on the contributing and our code of conduct.

## Versioning

For the versions available, see the [tags on this repository](https://github.com/metaguardpte/ZeroPass-Server/tags). 

## Releases

For the releases available, see the [releases on this repository](https://github.com/metaguardpte/ZeroPass-Server/releases). 

## Authors

See the list of [contributors](https://github.com/metaguardpte/ZeroPass-Server/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

# Braspag SDK para .NET Standard

SDK para integra��o simplificada nos servi�os da plataforma [Braspag](http://www.braspag.com.br/#solucoes)


[app-metapackage-nuget]: https://nuget.org/packages/Braspag.Sdk/
[app-metapackage-nuget-badge]: http://img.shields.io/nuget/v/Braspag.Sdk.svg?style=flat-square&label=Braspag.SDK

| Develop | Master | NuGet.org
|---|---|---|
| [![Build status](https://braspag.visualstudio.com/Innovation/_apis/build/status/Braspag-DotNet-SDK?branchName=develop)](https://braspag.visualstudio.com/Innovation/_build/latest?definitionId=470) | [![Build status](https://braspag.visualstudio.com/Innovation/_apis/build/status/Braspag-DotNet-SDK?branchName=master)](https://braspag.visualstudio.com/Innovation/_build/latest?definitionId=470) | [![][app-metapackage-nuget-badge]][app-metapackage-nuget]

> Para documenta��o completa das APIs e manuais, acesse <a href="http://braspag.github.io/" target="blank">http://braspag.github.io/</a>

## �ndice

- [Features](#features)
- [Instala��o](#instalacao)
- [Exemplos de Uso](#exemplos-de-uso)
  - [Pagador](#pagador)
  - [Cart�o Protegido](#cartao-protegido):
  - [Velocity](#velocity)

## Features

* Assembly para .NET Standard 2.0
* Instala��o simplificada utilizando [NuGet](https://www.nuget.org/packages/Braspag.Sdk/), sem necessidade de arquivos de configura��o
* Endpoints Braspag j� configurados no pacote
* Sele��o de ambientes Sandbox ou Production
* M�todos ass�ncronos para melhor desempenho nas requisi��es
* Client para a API Braspag Auth (Obten��o de tokens de acesso)
* Client para a API do Pagador (Autoriza��o, Captura, Cancelamento/Estorno, Consulta)
* Client para a API do Cart�o Protegido (Salvar cart�o, Recuperar cart�o, Invalidar cart�o)
* Client para a API de an�lises do Velocity

## Instala��o

Package Manager:

```xml
PM> Install-Package Braspag.Sdk
```


Interface de Linha de Comando (.NET Core CLI):

```xml
dotnet add package Braspag.Sdk
```


## Exemplos de Uso

### Pagador

```csharp
/* Cria��o do Cliente Pagador */
var pagadorClient = new PagadorClient(new PagadorClientOptions
{
    Environment = Environment.Sandbox,
    Credentials = new MerchantCredentials
    {
        MerchantId = "ID_DA_LOJA",
        MerchantKey = "CHAVE_DA_LOJA"
    }
});

/* Cria��o da requisi��o para nova transa��o */
var request = new SaleRequest
{
    MerchantOrderId = "123456789",
    Customer = new CustomerData
    {
        Name = "Bjorn Ironside",
        Birthdate = "1990-12-25",
        IdentityType = "CPF",
        Identity = "762.502.520-96",
        Email = "bjorn.ironside@vikings.com.br"
    },
    Payment = new PaymentDataRequest
    {
        Provider = "Simulado",
        Type = "CreditCard",
        Amount = 150000,
        ServiceTaxAmount = 0,
        Currency = "BRL",
        Country = "BRA",
        Installments = 1,
        SoftDescriptor = "Braspag SDK",
        CreditCard = new CreditCardData
        {
            CardNumber = "4485623136297301",
            Holder = "BJORN IRONSIDE",
            ExpirationDate = "12/2025",
            SecurityCode = "123",
            Brand = "visa"
        }
    }
};

/* Obten��o do resultado da opera��o */
var response = await pagadorClient.CreateSaleAsync(request);

```


### Cart�o Protegido

```csharp
/* Cria��o do Cliente Cart�o Protegido */
var cartaoProtegidoClient = new CartaoProtegidoClient(new CartaoProtegidoClientOptions
{
    Environment = Environment.Sandbox,
    Credentials = new MerchantCredentials
    {
        MerchantKey = "CHAVE_DA_LOJA"
    }
});

/* Salvar cart�o em cofre PCI */
var request = new SaveCreditCardRequest
{
    CustomerName = "Bjorn Ironside",
    CustomerIdentification = "762.502.520-96",
    CardHolder = "BJORN IRONSIDE",
    CardExpiration = "10/2025",
    CardNumber = "1000100010001000"
};

/* Obten��o do resultado da opera��o */
var response = await cartaoProtegidoClient.SaveCreditCardAsync(request);

```

### Velocity

```csharp
/* Cria��o do Token de Acesso OAUTH via Braspag Auth */
var braspagAuthClient = new BraspagAuthClient(new BraspagAuthClientOptions
{
    Environment = Environment.Sandbox
});

var authRequest = new AccessTokenRequest
{
    GrantType = OAuthGrantType.ClientCredentials,
    ClientId = "CLIENT_ID",
    ClientSecret = "CLIENT_SECRET",
    Scope = "VelocityApp"
};

/* Obten��o do token de acesso */
var authResponse = braspagAuthClient.CreateAccessTokenAsync(authRequest);

/* Cria��o do Cliente Velocity */
var velocityClient = new VelocityClient(new VelocityClientOptions
{
    Environment = Environment.Sandbox,
    Credentials = new MerchantCredentials
    {
        MerchantId = "ID_DA_LOJA",
        AccessToken = authResponse.Token
    }
});

/* Analisando uma transa��o com Velocity */
var request = new AnalysisRequest
{
    Transaction = new TransactionData
    {
        OrderId = DateTime.Now.Ticks.ToString(),
        Date = DateTime.UtcNow.ToString("O"),
        Amount = 1000
    },
    Card = new CardData
    {
        Holder = "BJORN IRONSIDE",
        Brand = "visa",
        Number = "1000100010001000",
        Expiration = "10/2025"
    },
    Customer = new CustomerData
    {
        Name = "Bjorn Ironside",
        Identity = "76250252096",
        IpAddress = "127.0.0.1",
        Email = "bjorn.ironside@vikings.com.br",
        BirthDate = "1982-06-30",
        Phones = new List<PhoneData>
        {
            new PhoneData
            {
                Type = "Cellphone",
                Number = "999999999",
                Ddi = "55",
                Ddd = "11"
            }
        },
        Billing = new AddressData
        {
            Street = "Alameda Xingu",
            Number = "512",
            Neighborhood = "Alphaville",
            City = "Barueri",
            State = "SP",
            Country = "BR",
            ZipCode = "06455-030"
        }
    }
};

/* Obten��o do resultado da opera��o */
var response = await velocityClient.PerformAnalysisAsync(request);
```
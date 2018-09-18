# Braspag SDK para .NET Standard

SDK para integração simplificada nos serviços da plataforma [Braspag](http://www.braspag.com.br/#solucoes)


[app-metapackage-nuget]: https://nuget.org/packages/Braspag.Sdk/
[app-metapackage-nuget-badge]: http://img.shields.io/nuget/v/Braspag.Sdk.svg?style=flat-square&label=Braspag.SDK

| Develop | Master | NuGet.org
|---|---|---|
| [![Build status](https://braspag.visualstudio.com/Innovation/_apis/build/status/Braspag-DotNet-SDK?branchName=develop)](https://braspag.visualstudio.com/Innovation/_build/latest?definitionId=470) | [![Build status](https://braspag.visualstudio.com/Innovation/_apis/build/status/Braspag-DotNet-SDK?branchName=master)](https://braspag.visualstudio.com/Innovation/_build/latest?definitionId=470) | [![][app-metapackage-nuget-badge]][app-metapackage-nuget]

> Para documentação completa das APIs e manuais, acesse <a href="http://braspag.github.io/" target="blank">http://braspag.github.io/</a>

## Índice

- [Features](#features)
- [Instalação](#instalacao)
- [Exemplos de Uso](#exemplos-de-uso)
  - [Pagador](#pagador)
  - [Cartão Protegido](#cartao-protegido):
  - [Velocity](#velocity)

## Features

* Assembly para .NET Standard 2.0
* Instalação simplificada utilizando [NuGet](https://www.nuget.org/packages/Braspag.Sdk/), sem necessidade de arquivos de configuração
* Endpoints Braspag já configurados no pacote
* Seleção de ambientes Sandbox ou Production
* Métodos assíncronos para melhor desempenho nas requisições
* Client para a API Braspag Auth (Obtenção de tokens de acesso)
* Client para a API do Pagador (Autorização, Captura, Cancelamento/Estorno, Consulta)
* Client para a API do Cartão Protegido (Salvar cartão, Recuperar cartão, Invalidar cartão)
* Client para a API de análises do Velocity

## Instalação

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
/* Criação do Cliente Pagador */
var pagadorClient = new PagadorClient(new PagadorClientOptions
{
    Environment = Environment.Sandbox,
    Credentials = new MerchantCredentials
    {
        MerchantId = "ID_DA_LOJA",
        MerchantKey = "CHAVE_DA_LOJA"
    }
});

/* Criação da requisição para nova transação */
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

/* Obtenção do resultado da operação */
var response = await pagadorClient.CreateSaleAsync(request);

```


### Cartão Protegido

```csharp
/* Criação do Cliente Cartão Protegido */
var cartaoProtegidoClient = new CartaoProtegidoClient(new CartaoProtegidoClientOptions
{
    Environment = Environment.Sandbox,
    Credentials = new MerchantCredentials
    {
        MerchantKey = "CHAVE_DA_LOJA"
    }
});

/* Salvar cartão em cofre PCI */
var request = new SaveCreditCardRequest
{
    CustomerName = "Bjorn Ironside",
    CustomerIdentification = "762.502.520-96",
    CardHolder = "BJORN IRONSIDE",
    CardExpiration = "10/2025",
    CardNumber = "1000100010001000"
};

/* Obtenção do resultado da operação */
var response = await cartaoProtegidoClient.SaveCreditCardAsync(request);

```

### Velocity

```csharp
/* Criação do Token de Acesso OAUTH via Braspag Auth */
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

/* Obtenção do token de acesso */
var authResponse = braspagAuthClient.CreateAccessTokenAsync(authRequest);

/* Criação do Cliente Velocity */
var velocityClient = new VelocityClient(new VelocityClientOptions
{
    Environment = Environment.Sandbox,
    Credentials = new MerchantCredentials
    {
        MerchantId = "ID_DA_LOJA",
        AccessToken = authResponse.Token
    }
});

/* Analisando uma transação com Velocity */
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

/* Obtenção do resultado da operação */
var response = await velocityClient.PerformAnalysisAsync(request);
```
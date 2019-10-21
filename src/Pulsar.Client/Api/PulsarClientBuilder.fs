﻿namespace Pulsar.Client.Api

open Pulsar.Client.Common
open System

type PulsarClientBuilder private (config: PulsarClientConfiguration) =

    let clientExceptionIfBlankString message arg =
        arg
        |> invalidArgIfBlankString message

    let verify(config : PulsarClientConfiguration) =
        let checkValue check config =
            check config |> ignore
            config

        config
        |> checkValue
            (fun c ->
                c.ServiceUrl
                |> clientExceptionIfBlankString "Service Url needs to be specified on the PulsarClientBuilder object.")

    new() = PulsarClientBuilder(PulsarClientConfiguration.Default)

    member this.ServiceUrl url =
        PulsarClientBuilder
            { config with
                ServiceUrl = url |> invalidArgIfBlankString "ServiceUrl must not be blank." }

    member this.MaxNumberOfRejectedRequestPerConnection num =
        PulsarClientBuilder
            { config with
                MaxNumberOfRejectedRequestPerConnection = num |> invalidArgIfLessThanZero "MaxNumberOfRejectedRequestPerConnection can't be negative" }

    member this.Authentication authentication =
        PulsarClientBuilder
            { config with
                Authentication = authentication |> invalidArgIfDefault "Authentication can't be null" }

    member this.Build() =
        config
        |> verify
        |> PulsarClient

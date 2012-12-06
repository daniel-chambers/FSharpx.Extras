﻿module FSharpx.TypeProviders.Tests.JsonZipper.ZipperTests

open NUnit.Framework
open FSharpx
open FsUnit

type Simple = JsonZipper<Schema="""{ "a": "b"}""">

[<Test>]
let ``Can create a zipper from inlined JSON``() = 
    let original = new Simple()
    original.ToString() |> should equal """{"a":"b"}"""

[<Test>]
let ``Can update a text property in a simple JSON without changing the original``() = 
    let original = new Simple()
    let updated = original.A.Update("c")
    updated.ToString() |> should equal """{"a":"c"}"""
    original.ToString() |> should equal """{"a":"b"}"""

type Simple1 = JsonZipper<Schema="""{ "b": 1}""">

[<Test>]
let ``Can update a int property in a simple JSON``() = 
    let original = new Simple1()
    let updated = original.B.Update(2)
    updated.ToString() |> should equal """{"b":2}"""
    original.ToString() |> should equal """{"b":1}"""

type Simple2 = JsonZipper<Schema="""{ "a": "b", "b": 1}""">

[<Test>]
let ``Can update two properties in a simple JSON``() = 
    let original = new Simple2()
    let updated = original.B.Update(2).Up().A.Update("blub")
    updated.ToString() |> should equal """{"a":"blub","b":2}"""
    original.ToString() |> should equal """{"a":"b","b":1}"""
    
type Nested = JsonZipper<Schema="""{ "a": "b", "b": { "c": "text" }}""">

[<Test>]
let ``Can update a property in a nested JSON``() = 
    let original = new Nested()
    let updated = original.B.C.Update("blub")
    updated.ToString() |> should equal """{"a":"b","b":{"c":"blub"}}"""
    original.ToString() |> should equal """{"a":"b","b":{"c":"text"}}"""

type DoubleNested = JsonZipper<Schema="""{ "a": "b", "b": { "c": { "d" : "down here" } }}""">

[<Test>]
let ``Can update a property in a deeply nested JSON``() = 
    let original = new DoubleNested()
    let updated = original.B.C.D.Update("blub")
    updated.ToString() |> should equal """{"a":"b","b":{"c":{"d":"blub"}}}"""
    original.ToString() |> should equal """{"a":"b","b":{"c":{"d":"down here"}}}"""


[<Test>]
let ``Can access toString in a nested JSON``() = 
    let original = new DoubleNested()
    original.B.ToString() |> should equal """{"a":"b","b":{"c":{"d":"down here"}}}"""
    original.B.C.ToString() |> should equal """{"a":"b","b":{"c":{"d":"down here"}}}"""
    original.ToString() |> should equal """{"a":"b","b":{"c":{"d":"down here"}}}"""
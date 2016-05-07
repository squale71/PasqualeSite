# CoffeeScript

#PasqualeSite Namespace Setup
namespace = (name) ->
    if not window.PasqualeSite then window.PasqualeSite = {}
    PasqualeSite[name] = PasqualeSite[name] or {}

#Page Namespace
namespace "ViewModels"

#PasqualeSite.KnockoutModel: Create a standard Knockout Model     
class PasqualeSite.KnockoutModel 
    constructor: (JSON, ViewModel) ->
        JSON.binding = true     
        if ViewModel
            @Model = ViewModel
            
        ko.mapping.merge.fromJS(@Model, JSON)  
        ko.applyBindings @Model

        @Model  

class PasqualeSite.ViewModels.PostsViewModel
    constructor: () ->
        @Posts = ko.observableArray([]).withMergeConstructor(PasqualeSite.ViewModels.PostViewModel, true)  

        @SelectedPost = ko.observableArray([]).withMergeConstructor(PasqualeSite.ViewModels.PostViewModel, true)  

class PasqualeSite.ViewModels.PostViewModel
    constructor: () ->
        @Id = ko.observable()
        @Title = ko.observable()
        @Teaser = ko.observable()
        @PostContent = ko.observable()
        @DateCreated = ko.observable()
        @DateModified = ko.observable()
        @isFeatured = ko.observable()
        @Priority = ko.observable()

        @User = ko.observable()
        @Image = ko.observable()

$ ->
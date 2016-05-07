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
        @NewPost = ko.observable(new PasqualeSite.ViewModels.PostViewModel())
        @GridOptions = 
            data: @Posts
            columnDefs: [
                { field: 'Id', displayName: 'Id', width: 90 }
                { field: 'Title', displayName: 'Title', width: 80 }
                { field: 'Teaser', cellClass: 'Teaser', headerClass: 'Teaser' }
                { field: 'DateCreated', displayName: 'Date Created'}
                { field: 'DateModified', displayName: 'Date Modified'}
                { field: 'isFeatured', displayName: 'Featured'}
                { field: 'Priority', displayName: 'Priority'}
            ]
            enablePaging: true
            selectedItems: @SelectedPost
            multiSelect: false   
            afterSelectionChange: =>
                #CKEDITOR.replace('content');  
            pagingOptions: 
                currentPage: ko.observable(1)
                pageSize: ko.observable(30)
                pageSizes: ko.observableArray([30, 60, 90])
                totalServerItems: ko.observable(0) 

class PasqualeSite.ViewModels.PostViewModel
    constructor: () ->
        @Id = ko.observable()
        @Title = ko.observable()
        @Teaser = ko.observable()
        @PostContent = ko.observable("")
        @DateCreated = ko.observable()
        @DateModified = ko.observable()
        @isFeatured = ko.observable()
        @Priority = ko.observable()

        @User = ko.observable()
        @Image = ko.observable()

        @FeaturePost = () =>
            @isFeatured(true)

        @UnfeaturePost = () =>
            @isFeatured(false)

$ ->
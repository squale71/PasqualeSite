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
        @Tags = ko.observableArray([]).withMergeConstructor(PasqualeSite.ViewModels.TagViewModel, true)  

        @SelectedPost = ko.observableArray([]).withMergeConstructor(PasqualeSite.ViewModels.PostViewModel, true)  

        @SelectedTag = ko.observableArray() 
        @NewPost = ko.observable(new PasqualeSite.ViewModels.PostViewModel())
        @PostGridOptions = 
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
                CKEDITOR.instances['content'].setData(@SelectedPost()[0].PostContent()) #Populates CKEditor with content.
                #CKEDITOR.replace('content');  
            pagingOptions: 
                currentPage: ko.observable(1)
                pageSize: ko.observable(30)
                pageSizes: ko.observableArray([30, 60, 90])
                totalServerItems: ko.observable(0) 

        @TagGridOptions =
            data: @Tags
            columnDefs: [
                { field: 'Id', displayName: 'Id', width: 100}
                { field: 'Name', displayName: 'Name', width: 200}
            ]
            enablePaging: true
            selectedItems: @SelectedTag
            multiSelect: false  
            disableTextSelection: false 
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
        @IsFeatured = ko.observable()
        @IsActive = ko.observable()
        @Priority = ko.observable()

        @User = ko.observable(new PasqualeSite.ViewModels.AppUser())
        @Image = ko.observable(new PasqualeSite.ViewModels.PostImageViewModel())

        @FeaturePost = () =>
            @IsFeatured(true)

        @UnfeaturePost = () =>
            @IsFeatured(false)

        @EnablePost = () =>
            @IsActive(true)

        @DisablePost = () =>
            @IsActive(false)

        @SavePost = (isExistingPost) =>
            newPost = 
                Id: @Id()
                Title: @Title()
                Teaser: @Teaser()
                PostContent: @PostContent()
                DateCreated: @DateCreated()
                DateModified: @DateModified()
                IsFeatured: @IsFeatured()
                IsActive: @IsActive()
                Priority: @Priority()
                ImageId: if @Image() then @Image().Id() else null
            $.ajax({
                url: approot + "Admin/SavePost"
                data: newPost: newPost
                type: "POST",
                success: (data) =>
                    if isExistingPost
                        model.Model.Posts.push({})
                        model.Model.Posts.pop()
                        PasqualeSite.notify("Success Updating Post", "success")
                    else
                        model.Model.NewPost(new PasqualeSite.ViewModels.PostViewModel())
                        PasqualeSite.notify("Success Adding New Post", "success")
                error: (err) =>
                    PasqualeSite.notify("There was a problem saving the post", "error")
            }) 

class PasqualeSite.ViewModels.TagViewModel
    constructor: () ->
        @Id = ko.observable()
        @Name = ko.observable()


class PasqualeSite.ViewModels.PostImageViewModel
    constructor: () ->
        @Id = ko.observable()
        @Path = ko.observable()
        @Description = ko.observable()


class PasqualeSite.ViewModels.AppUser
    constructor: () ->
        @Id = ko.observable()
        @FirstName = ko.observable()
        @LastName = ko.observable()
        @UserName = ko.observable()
        @Email = ko.observable()
        @EmailConfirmed = ko.observable()
        @Roles = ko.observableArray([])





$ ->
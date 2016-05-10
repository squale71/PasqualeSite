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
        @Images = ko.observableArray([]).withMergeConstructor(PasqualeSite.ViewModels.PostImageViewModel, true) 

        @SelectedPost = ko.observableArray([]).withMergeConstructor(PasqualeSite.ViewModels.PostViewModel, true)  
        @SelectedTag = ko.observableArray([]) 
        @SelectedImage = ko.observableArray([])

        @NewPost = ko.observable(new PasqualeSite.ViewModels.PostViewModel())
        @NewTag = ko.observable()
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

        @ImageGridOptions = 
            data: @Images
            columnDefs: [
                { field: 'Name', displayName: 'Name', width: 500}
                { field: 'PathHtml', displayName: 'Path', width: 500}
                { field: 'CopyButton', displayName: 'Copy', width: 50 }
                { field: 'Link', displayName: 'Link', width: 50 }                
            ]
            enablePaging: true
            multiSelect: false  
            disableTextSelection: false 
            rowHeight: 50
            selectWithCheckboxOnly: true
            pagingOptions: 
                currentPage: ko.observable(1)
                pageSize: ko.observable(5)
                pageSizes: ko.observableArray([5, 10, 15])
                totalServerItems: ko.observable(0) 


        @AddTag = () =>
            if @NewTag() && @NewTag().trim() != ""
                $.ajax({
                    url: approot + "Admin/AddTag"
                    data: name: @NewTag()
                    type: "POST",
                    success: (data) =>
                        json = JSON.parse(data)
                        theTag = new PasqualeSite.ViewModels.TagViewModel()
                        ko.mapping.merge.fromJS(theTag, json)  

                        @Tags.push(theTag)
                        PasqualeSite.notify("Success Adding New Tag", "success")
                    error: (err) =>
                        PasqualeSite.notify("There was a problem saving the tag", "error")
                }) 
            else 
                PasqualeSite.notify("You need to fill in a value in order to add a tag.", "info")


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

                        model.Model.SelectedPost.removeAll()
                        PasqualeSite.notify("Success Updating Post", "success")
                    else
                        json = JSON.parse(data)
                        post = new PasqualeSite.ViewModels.PostViewModel()
                        ko.mapping.merge.fromJS(post, json)  
                        
                        model.Model.Posts.push(post)
                        model.Model.NewPost(new PasqualeSite.ViewModels.PostViewModel())
                        PasqualeSite.notify("Success Adding New Post", "success")
                error: (err) =>
                    PasqualeSite.notify("There was a problem saving the post", "error")
            }) 

        @DeletePost = () =>
            confirmation = confirm "Are you sure you want to delete the post? This action cannot be undone."
            if confirmation
                $.ajax({
                    url: approot + "Admin/DeletePost"
                    data: id: @Id()
                    type: "POST",
                    success: (data) =>
                        model.Model.Posts.remove(@)
                        PasqualeSite.notify("Success Removing Post", "success")
                    error: (err) =>
                        PasqualeSite.notify("There was a problem removing the post", "error")
                }) 

class PasqualeSite.ViewModels.TagViewModel
    constructor: () ->
        @Id = ko.observable()
        @Name = ko.observable()

        @SaveTag = () =>
            newTag = 
                Id: @Id()
                Name: @Name()
            $.ajax({
                url: approot + "Admin/SaveTag"
                data: newTag: newTag
                type: "POST",
                success: (data) =>
                    model.Model.Tags.push({})
                    model.Model.Tags.pop()
                    PasqualeSite.notify("Success Updating Tag", "success")
                error: (err) =>
                    PasqualeSite.notify("There was a problem saving the tag", "error")
            }) 

        @DeleteTag = () =>
            $.ajax({
                url: approot + "Admin/DeleteTag"
                data: id: @Id()
                type: "POST",
                success: (data) =>
                    model.Model.Tags.remove(@)
                    PasqualeSite.notify("Success Deleting Tag", "success")
                error: (err) =>
                    PasqualeSite.notify("There was a problem deleting the tag", "error")
            }) 

class PasqualeSite.ViewModels.PostImageViewModel
    constructor: () ->
        @Id = ko.observable()
        @Name = ko.observable()
        @Path = ko.observable()

        @PathHtml = ko.computed( =>
            return "<input class='form-control' type='text' value='" + @Path() + "' id='link-" + @Name() + "' />"
        ,this)

        @Link = ko.computed( =>
            return "<a target='_blank' href='" + @Path() + "'><span class='glyphicon glyphicon-picture'></span></a>"
        ,this)

        @CopyButton = ko.computed( =>
            return "<button class='btn btn-default' data-copytarget='#link-" + @Name() + "'><span class='glyphicon glyphicon-copy'></span></button>"
        ,this)

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
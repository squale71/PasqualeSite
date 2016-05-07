# CoffeeScript

#PasqualeSite Namespace Setup
namespace = (name) ->
    if not window.PasqualeSite then window.PasqualeSite = {}
    PasqualeSite[name] = PasqualeSite[name] or {}

#Page Namespace
namespace "ViewModels"

#PasqualeSite.KnockoutModel: Create a standard Knockout Model     
class PasqualeSite.KnockoutModel 
    constructor: (JSON, ChangeCallback, ViewModel, SkipRebind, SkipValidation, IncludeDirtyFlag) ->
        JSON.binding = true     
        if ViewModel
            @Model = ViewModel
            
        ko.mapping.merge.fromJS(@Model, JSON)  
        ko.applyBindings @Model  unless SkipRebind

        @Model  



$ ->
ko.bindingHandlers.CKEDITOR = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        var ckEditorValue = valueAccessor();
        var id = $(element).attr('id');
        var options = allBindings().EditorOptions;
        var ignoreChanges = false;

        var instance = CKEDITOR.replace(id, {
            on: {
                change: function () {
                    ignoreChanges = true;
                    ckEditorValue(instance.getData());
                    ignoreChanges = false;
                }
            }
        });

        ckEditorValue.subscribe(function (newValue) {
            if (!ignoreChanges) {
                instance.setData(newValue);
            }
        });

    }
};
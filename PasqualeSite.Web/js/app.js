﻿// Generated by IcedCoffeeScript 108.0.9
(function() {
  var namespace;

  namespace = function(name) {
    if (!window.PasqualeSite) {
      window.PasqualeSite = {};
    }
    return PasqualeSite[name] = PasqualeSite[name] || {};
  };

  namespace("ViewModels");

  PasqualeSite.KnockoutModel = (function() {
    function KnockoutModel(JSON, ViewModel) {
      JSON.binding = true;
      if (ViewModel) {
        this.Model = ViewModel;
      }
      ko.mapping.merge.fromJS(this.Model, JSON);
      ko.applyBindings(this.Model);
      this.Model;
    }

    return KnockoutModel;

  })();

  PasqualeSite.ViewModels.PostsViewModel = (function() {
    function PostsViewModel() {
      this.Posts = ko.observableArray([]).withMergeConstructor(PasqualeSite.ViewModels.PostViewModel, true);
      this.Tags = ko.observableArray([]).withMergeConstructor(PasqualeSite.ViewModels.TagViewModel, true);
      this.Images = ko.observableArray([]).withMergeConstructor(PasqualeSite.ViewModels.PostImageViewModel, true);
      this.SelectedPost = ko.observableArray([]).withMergeConstructor(PasqualeSite.ViewModels.PostViewModel, true);
      this.SelectedTag = ko.observableArray([]);
      this.SelectedImage = ko.observableArray([]);
      this.NewPost = ko.observable(new PasqualeSite.ViewModels.PostViewModel());
      this.NewTag = ko.observable();
      this.PostGridOptions = {
        data: this.Posts,
        columnDefs: [
          {
            field: 'Id',
            displayName: 'Id',
            width: 50
          }, {
            field: 'Title',
            displayName: 'Title',
            width: 200
          }, {
            field: 'Teaser',
            cellClass: 'Teaser',
            headerClass: 'Teaser',
            width: 750
          }, {
            field: 'FriendlyDateCreated',
            displayName: 'Date Created',
            width: 250
          }, {
            field: 'FriendlyDateModified',
            displayName: 'Date Modified',
            width: 250
          }, {
            field: 'FeaturedText',
            displayName: 'Featured',
            width: 100
          }, {
            field: 'Priority',
            displayName: 'Priority',
            width: 100
          }
        ],
        enablePaging: true,
        selectedItems: this.SelectedPost,
        multiSelect: false,
        afterSelectionChange: (function(_this) {
          return function() {
            $('#postGrid').hide();
            $('#editPost').show();
            return CKEDITOR.instances['content'].setData(_this.SelectedPost()[0].PostContent());
          };
        })(this),
        pagingOptions: {
          currentPage: ko.observable(1),
          pageSize: ko.observable(30),
          pageSizes: ko.observableArray([30, 60, 90]),
          totalServerItems: ko.observable(0)
        }
      };
      this.TagGridOptions = {
        data: this.Tags,
        columnDefs: [
          {
            field: 'Id',
            displayName: 'Id',
            width: 100
          }, {
            field: 'Name',
            displayName: 'Name',
            width: 200
          }
        ],
        enablePaging: true,
        selectedItems: this.SelectedTag,
        multiSelect: false,
        afterSelectionChange: (function(_this) {
          return function() {
            $('#tagGrid').hide();
            return $('#editTag').show();
          };
        })(this),
        disableTextSelection: false,
        pagingOptions: {
          currentPage: ko.observable(1),
          pageSize: ko.observable(30),
          pageSizes: ko.observableArray([30, 60, 90]),
          totalServerItems: ko.observable(0)
        }
      };
      this.ImageGridOptions = {
        data: this.Images,
        columnDefs: [
          {
            field: 'Link',
            displayName: 'Link',
            width: 50
          }, {
            field: 'Name',
            displayName: 'Name',
            width: 500
          }, {
            field: 'PathHtml',
            displayName: 'Path',
            width: 500
          }, {
            field: 'CopyButton',
            displayName: 'Copy',
            width: 50
          }
        ],
        enablePaging: true,
        multiSelect: false,
        disableTextSelection: false,
        rowHeight: 50,
        selectWithCheckboxOnly: true,
        pagingOptions: {
          currentPage: ko.observable(1),
          pageSize: ko.observable(5),
          pageSizes: ko.observableArray([5, 10, 15]),
          totalServerItems: ko.observable(0)
        }
      };
      this.BackToPostGrid = (function(_this) {
        return function() {
          $('#editPost').hide();
          return $('#postGrid').show();
        };
      })(this);
      this.BackToTagGrid = (function(_this) {
        return function() {
          $('#editTag').hide();
          return $('#tagGrid').show();
        };
      })(this);
      this.AddTag = (function(_this) {
        return function() {
          if (_this.NewTag() && _this.NewTag().trim() !== "") {
            return $.ajax({
              url: approot + "Admin/AddTag",
              data: {
                name: _this.NewTag()
              },
              type: "POST",
              success: function(data) {
                var json, theTag;
                json = JSON.parse(data);
                theTag = new PasqualeSite.ViewModels.TagViewModel();
                ko.mapping.merge.fromJS(theTag, json);
                _this.Tags.push(theTag);
                return PasqualeSite.notify("Success Adding New Tag", "success");
              },
              error: function(err) {
                return PasqualeSite.notify("There was a problem saving the tag", "error");
              }
            });
          } else {
            return PasqualeSite.notify("You need to fill in a value in order to add a tag.", "info");
          }
        };
      })(this);
    }

    return PostsViewModel;

  })();

  PasqualeSite.ViewModels.PostViewModel = (function() {
    function PostViewModel() {
      this.Id = ko.observable();
      this.Title = ko.observable();
      this.Teaser = ko.observable();
      this.PostContent = ko.observable("");
      this.DateCreated = ko.observable();
      this.DateModified = ko.observable();
      this.IsFeatured = ko.observable();
      this.IsActive = ko.observable();
      this.Priority = ko.observable();
      this.FriendlyDateCreated = ko.computed((function(_this) {
        return function() {
          var newDate;
          if (_this.DateCreated()) {
            newDate = Date.parse(_this.DateCreated().substr(0, 19));
            return newDate.toString('MM/d/yyyy, hh:mm tt');
          }
        };
      })(this), this);
      this.FriendlyDateModified = ko.computed((function(_this) {
        return function() {
          var newDate;
          if (_this.DateModified()) {
            newDate = Date.parse(_this.DateModified().substr(0, 19));
            return newDate.toString('MM/d/yyyy, hh:mm tt');
          }
        };
      })(this), this);
      this.FeaturedText = ko.computed((function(_this) {
        return function() {
          if (_this.IsFeatured() === true) {
            return "Yes";
          } else {
            return "No";
          }
        };
      })(this), this);
      this.User = ko.observable(new PasqualeSite.ViewModels.AppUser());
      this.ImageId = ko.observable();
      this.PostTags = ko.observableArray([]).withMergeConstructor(PasqualeSite.ViewModels.PostTagViewModel, true);
      this.TagIds = ko.observableArray([]);
      this.FeaturePost = (function(_this) {
        return function() {
          return _this.IsFeatured(true);
        };
      })(this);
      this.UnfeaturePost = (function(_this) {
        return function() {
          return _this.IsFeatured(false);
        };
      })(this);
      this.EnablePost = (function(_this) {
        return function() {
          return _this.IsActive(true);
        };
      })(this);
      this.DisablePost = (function(_this) {
        return function() {
          return _this.IsActive(false);
        };
      })(this);
      this.SavePost = (function(_this) {
        return function(isExistingPost) {
          var newPost;
          newPost = {
            Id: _this.Id(),
            Title: _this.Title(),
            Teaser: _this.Teaser(),
            PostContent: _this.PostContent(),
            DateCreated: _this.DateCreated(),
            DateModified: _this.DateModified(),
            IsFeatured: _this.IsFeatured(),
            IsActive: _this.IsActive(),
            Priority: _this.Priority(),
            TagIds: _this.TagIds(),
            ImageId: _this.ImageId() ? _this.ImageId() : null
          };
          return $.ajax({
            url: approot + "Admin/SavePost",
            data: {
              newPost: newPost
            },
            type: "POST",
            success: function(data) {
              var json, post;
              if (isExistingPost) {
                model.Model.Posts.push({});
                model.Model.Posts.pop();
                model.Model.SelectedPost.removeAll();
                model.Model.BackToPostGrid();
                return PasqualeSite.notify("Success Updating Post", "success");
              } else {
                json = JSON.parse(data);
                post = new PasqualeSite.ViewModels.PostViewModel();
                ko.mapping.merge.fromJS(post, json);
                model.Model.Posts.push(post);
                model.Model.NewPost(new PasqualeSite.ViewModels.PostViewModel());
                return PasqualeSite.notify("Success Adding New Post", "success");
              }
            },
            error: function(err) {
              return PasqualeSite.notify("There was a problem saving the post", "error");
            }
          });
        };
      })(this);
      this.DeletePost = (function(_this) {
        return function() {
          var confirmation;
          confirmation = confirm("Are you sure you want to delete the post? This action cannot be undone.");
          if (confirmation) {
            return $.ajax({
              url: approot + "Admin/DeletePost",
              data: {
                id: _this.Id()
              },
              type: "POST",
              success: function(data) {
                model.Model.Posts.remove(_this);
                return PasqualeSite.notify("Success Removing Post", "success");
              },
              error: function(err) {
                return PasqualeSite.notify("There was a problem removing the post", "error");
              }
            });
          }
        };
      })(this);
    }

    return PostViewModel;

  })();

  PasqualeSite.ViewModels.PostTagViewModel = (function() {
    function PostTagViewModel() {
      this.Tag = ko.observable();
    }

    return PostTagViewModel;

  })();

  PasqualeSite.ViewModels.TagViewModel = (function() {
    function TagViewModel() {
      this.Id = ko.observable();
      this.Name = ko.observable();
      this.SaveTag = (function(_this) {
        return function() {
          var newTag;
          newTag = {
            Id: _this.Id(),
            Name: _this.Name()
          };
          return $.ajax({
            url: approot + "Admin/SaveTag",
            data: {
              newTag: newTag
            },
            type: "POST",
            success: function(data) {
              model.Model.Tags.push({});
              model.Model.Tags.pop();
              model.Model.BackToTagGrid();
              return PasqualeSite.notify("Success Updating Tag", "success");
            },
            error: function(err) {
              return PasqualeSite.notify("There was a problem saving the tag", "error");
            }
          });
        };
      })(this);
      this.DeleteTag = (function(_this) {
        return function() {
          return $.ajax({
            url: approot + "Admin/DeleteTag",
            data: {
              id: _this.Id()
            },
            type: "POST",
            success: function(data) {
              model.Model.Tags.remove(_this);
              model.Model.BackToTagGrid();
              return PasqualeSite.notify("Success Deleting Tag", "success");
            },
            error: function(err) {
              return PasqualeSite.notify("There was a problem deleting the tag", "error");
            }
          });
        };
      })(this);
    }

    return TagViewModel;

  })();

  PasqualeSite.ViewModels.PostImageViewModel = (function() {
    function PostImageViewModel() {
      this.Id = ko.observable();
      this.Name = ko.observable();
      this.Path = ko.observable();
      this.PathHtml = ko.computed((function(_this) {
        return function() {
          return "<input class='form-control' type='text' value='" + _this.Path() + "' id='link-" + _this.Name() + "' />";
        };
      })(this), this);
      this.Link = ko.computed((function(_this) {
        return function() {
          return "<a class='imgLink' target='_blank' href='" + _this.Path() + "'><span class='glyphicon glyphicon-picture'></span></a>";
        };
      })(this), this);
      this.CopyButton = ko.computed((function(_this) {
        return function() {
          return "<button class='btn btn-default' data-copytarget='#link-" + _this.Name() + "'><span class='glyphicon glyphicon-copy'></span></button>";
        };
      })(this), this);
      this.Description = ko.observable();
    }

    return PostImageViewModel;

  })();

  PasqualeSite.ViewModels.AppUser = (function() {
    function AppUser() {
      this.Id = ko.observable();
      this.FirstName = ko.observable();
      this.LastName = ko.observable();
      this.UserName = ko.observable();
      this.Email = ko.observable();
      this.EmailConfirmed = ko.observable();
      this.Roles = ko.observableArray([]);
    }

    return AppUser;

  })();

  $(function() {});

}).call(this);

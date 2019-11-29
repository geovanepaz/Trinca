angular
  .module("boilerplate")
  .directive('enforceMaxTags', function ()
  {
    var KEY_BACKSPACE = 8;
    return {
      require: 'ngModel',
      priority: -10,
      link: function ($scope, $element, $attrs, ngModelController)
      {
        var tagsInputScope = $element.isolateScope(),
          maxTags,
          getTags,
          checkTags,
          maxTagsReached,
          input = $element.find('input'),
          placeholder;

        $attrs.$observe('maxTags', function (_maxTags)
        {
          maxTags = _maxTags;
        });

        getTags = function ()
        {
          return ngModelController.$modelValue;
        };

        checkTags = function ()
        {
          var tags = getTags();
          if (tags && tags.length && tags.length >= maxTags)
          {
            placeholder = input.attr('placeholder');
            input.attr('placeholder', '');
            input.css('width', '10px');
            maxTagsReached = true;
          } else if (maxTagsReached)
          {
            input.attr('placeholder', placeholder);
            input.css('width', '');
            maxTagsReached = false;
          }
        };

        $scope.$watch(getTags, checkTags);

        // prevent any keys from being entered into
        // the input when max tags is reached
        input.on('keydown', function (event)
        {
          if (maxTagsReached && event.keyCode !== KEY_BACKSPACE)
          {
            event.stopImmediatePropagation();
            event.preventDefault();
          }
        });

        // prevent the autocomplete from being triggered
        input.on('focus', function (event)
        {
          checkTags();
          if (maxTagsReached)
          {
            tagsInputScope.hasFocus = true;
            event.stopImmediatePropagation();
          }
        });
      }
    };
  });

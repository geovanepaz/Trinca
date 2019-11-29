; (function ($)
{

  /**
   * In genereal you should avoid to use jQuery code in AngularJS
   * apps, if you need any jQuery functionality create a directive
   *
   */
  $(function ()
  {
    // showing modal with effect
    $('document').on('click', '.modal-effect', function (e)
    {
      e.preventDefault();
      var effect = $(this).attr('data-effect');
      $('#modaldemo8').addClass(effect);
    });

    // hide modal with effect
    $('#modaldemo8').on('hidden.bs.modal', function (e)
    {
      $(this).removeClass(function (index, className)
      {
        return (className.match(/(^|\s)effect-\S+/g) || []).join(' ');
      });
    });
  });


})(jQuery);

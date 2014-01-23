(function () {
    'use strict';

    var partialRoot = 'app/partials/';

    var appConfigCallback = function ($routeProvider) {
        $routeProvider.
            when('/', {
                templateUrl: partialRoot + 'main.html',
                controller: 'MainCtrl'
            }).
            when('/checks', {
                templateUrl: partialRoot + 'checks.html',
                controller: 'ChecksCtrl'
            }).
            otherwise({
                redirectTo: '/'
            });
    };

    var appDependencies = ['ngRoute',
                           'pingboard.controllers',
                           'pingboard.services'];

    var pingboard = angular.module('pingboard', appDependencies)
                           .config(['$routeProvider', appConfigCallback]);

    return pingboard;

})();
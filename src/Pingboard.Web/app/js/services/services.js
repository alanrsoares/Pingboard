(function () {
    'use strict';

    var dependencies = ['ngResource'];

    var services = angular.module('pingboard.services', dependencies);

    services.factory('checks', ['$resource', function ($resource) {
        return $resource('/api/checks/:checkId/', { checkId: "@checkId" }, {});
    }])
    .factory("analysis", ['$resource', function ($resource) {
        return $resource("/api/analysis/:analysisId/", { checkId: "@analysisId" });
    }])
    .factory("probes", ['$resource', function ($resource) {
        return $resource("/api/probes/");
    }])
    .factory("traceroute", ['$resource', function ($resource) {
        return $resource("/api/traceroute/");
    }]);

})();
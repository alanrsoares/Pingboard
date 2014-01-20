(function () {
    'use strict';

    angular.module('pingboard.services', ['ngResource'])
        .factory('checks', ['$resource', function ($resource) {
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
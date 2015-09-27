'use strict';

//inject service into app
angular
    .module('PBD')
    .factory('FriendRequestService', FriendRequestService);

FriendRequestService.$inject = ['$http', '$httpParamSerializer'];

function FriendRequestService($http, $httpParamSerializer) {

    return {
        GetFriendRequests: GetFriendRequests //,
        //more method declarations go here
    }

    function GetFriendRequests() {
        return $http.get('/Api/FriendRequests');
    }

    //More methods go here

}


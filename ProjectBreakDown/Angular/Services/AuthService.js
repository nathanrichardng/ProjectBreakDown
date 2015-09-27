'use strict';

//inject service into app
angular
    .module('PBD')
    .factory('AuthService', AuthService);

AuthService.$inject = ['$http', '$httpParamSerializer', '$q'];

function AuthService($http, $httpParamSerializer, $q) {

    var bearer_token = null;

    return {
        Token: Token,
        Authenticate: Authenticate
        //more method declarations go here
    }

    //Returns auth token
    function Token() {
        return bearer_token;
    }

    //Issue POST request to /token
    //Need to update this to use $q defer so a promise can be returned.
    function Authenticate(username, password) {

        var deferred = $q.defer();

        var postData = $httpParamSerializer({
            'grant_type': 'password',
            'username': username,
            'password': password
        });

        $http.post('/token', postData).then(successCallback, failCallback);

        return deferred.promise;

        //Callbacks
        function successCallback(response) {

            bearer_token = response.data.access_token;
            $http.defaults.headers.common.Authorization = "bearer " + bearer_token;
            //console.log(response);
            //console.log(bearer_token);
            deferred.resolve(response);

        };

        function failCallback(response) {

            console.log(response);

        };
    }


    //More methods go here

}


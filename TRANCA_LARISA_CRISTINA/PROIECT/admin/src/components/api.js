var api = {
    getResource(term){
      url = 'http://api.wunderground.com/api/7371ed5d87903525/geolookup/hourly/q/'+ term[1]+','+term[0]+'.json';
      let response =  fetch(url);
      return response.then((response) => response.json());
  },
  postResource(params){
    return fetch("http://www.example.co.uk/login", {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(params)
    })

        .then((response) => response.json())
        .then((responseData) => {
            console.log(
                "POST Response",
                "Response Body -> " + JSON.stringify(responseData)
            )
        })
        .done();
  }
}
module.exports = api;

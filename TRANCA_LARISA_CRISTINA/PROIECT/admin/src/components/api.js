var api = {
    getResource(term){
      // url = 'http://api.wunderground.com/api/7371ed5d87903525/geolookup/hourly/q/'+ term[1]+','+term[0]+'.json';
      // let response =  fetch(url);
      // return response.then((response) => response.json());
return fetch('http://api.wunderground.com/api/7371ed5d87903525/geolookup/hourly/q/'+ term[1]+','+term[0]+'.json')
      .then((response) => response.json())
    .then((responseJson) => {
      return responseJson;
    })
    .catch((error) => {
      console.error(error);
    });
  },
  postResource(params){
    return postAuth(params);
  },
  createAccount(params){
    return postCreate(params);
  },
  addResource(params){
    setResource(params);
    async function setResource(params){
      console.log(params);
      try{
      let response = await fetch('https://0c81bb1d.ngrok.io/add-resource', {
          method: 'POST',
          headers: {
              'Accept': 'application/json',
              'Content-Type': 'application/json'
          },
          body: JSON.stringify(params)
      });
      let responseJson = await response.json();
      console.log(responseJson);
      return responseJson.message;
      }
      catch(error){
        console.error(error);
      }
    }
    },
};

async function postAuth(params){
  try{
  let response = await fetch('https://0c81bb1d.ngrok.io/auth', {
      method: 'POST',
      headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
      },
      body: JSON.stringify(params)
  });
  let responseJson = await response.json();
  return responseJson.message;
  }
  catch(error){
    console.error(error);
  }
}

async function postCreate(params){
  try{
  let response = await fetch('https://0c81bb1d.ngrok.io/create-account', {
      method: 'POST',
      headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
      },
      body: JSON.stringify(params)
  });
  let responseJson = await response.json();
  return responseJson.message;
  }
  catch(error){
    console.error(error);
  }
}


module.exports = api;

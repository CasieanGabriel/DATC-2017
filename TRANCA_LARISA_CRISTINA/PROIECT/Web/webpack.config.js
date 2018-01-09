module.exports = {
    entry : [ 'babel-polyfill', './public/appPageRender.js' ],
    output :{
        path: __dirname + "/public",
        filename: "bundle.js"
    },
    module:{
        loaders:[
            {
                exclude: /(node_modules)/,
                loader: 'babel-loader',
                query:{
                    presets: ['env','react']
                }
            }
        ]
    },
    watch: true
}
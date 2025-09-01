const PROXY_CONFIG = {
  "/api": {
    "target": "http://199.247.23.165:7404",
    "secure": false,
    "changeOrigin": true,
    "logLevel": "debug",
    "pathRewrite": {
      "^/api": "/api/graphql"
    }
  }
};

module.exports = PROXY_CONFIG;

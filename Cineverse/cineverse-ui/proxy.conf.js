const PROXY_CONFIG = {
  "/cineverse-api": {
    "target": "http://localhost:7400  ",
    "secure": false,
    "changeOrigin": true,
    "pathRewrite": {
      "^/cineverse-api": "/api/graphql"
    }
  }
};

module.exports = PROXY_CONFIG;

(function () {
    const token = localStorage.getItem("jwt");
    if (token) {
        const authHeader = "Bearer " + token;
        window.swaggerUi.api.clientAuthorizations.add("Bearer", new SwaggerClient.ApiKeyAuthorization("Authorization", authHeader, "header"));
    }
})();


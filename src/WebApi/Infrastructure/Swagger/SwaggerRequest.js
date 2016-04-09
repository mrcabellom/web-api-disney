$(function () {
    window.version = 1;
    $("#input_baseUrl").on("change", function () {
        var fullVersion = $("#input_baseUrl option:selected").text();
        window.version = fullVersion[fullVersion.length - 1];
        window.swaggerUi.api.clientAuthorizations.add("x-api-version", new SwaggerClient.ApiKeyAuthorization('x-api-version', window.version, 'header'));
    });
});

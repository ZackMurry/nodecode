package com.zackmurry.nodecode.backend.model

data class PackageCreateRequest(
    var name: String,
    var description: String,
    var documentationUrl: String?,
    var repositoryUrl: String?
)

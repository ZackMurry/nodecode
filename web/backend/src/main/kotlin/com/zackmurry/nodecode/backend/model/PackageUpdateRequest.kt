package com.zackmurry.nodecode.backend.model

data class PackageUpdateRequest(
    var description: String,
    var documentationUrl: String?,
    var repositoryUrl: String?
)

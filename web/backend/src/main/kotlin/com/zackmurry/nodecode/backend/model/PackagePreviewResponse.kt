package com.zackmurry.nodecode.backend.model

import com.zackmurry.nodecode.backend.entity.Package

data class PackagePreviewResponse(
    var name: String,
    var description: String,
    var latestVersion: String?
) {
    companion object {
        fun from(pkg: Package): PackagePreviewResponse {
            return PackagePreviewResponse(
                pkg.name,
                pkg.description,
                pkg.latestVersion
            )
        }
    }
}

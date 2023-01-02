package com.zackmurry.nodecode.backend.model

import com.zackmurry.nodecode.backend.entity.Package

data class PackageResponse(
    var name: String,
    var author: UserResponse,
    var description: String,
    var lastUpdated: Long,
    var documentationUrl: String?,
    var repositoryUrl: String?,
    var downloads: Int,
    var latestVersion: String?,
    var versions: List<PackageVersionResponse>, // sorted: most recent is index 0
    var isOwnedByUser: Boolean
) {
    companion object {
        fun from(pkg: Package, author: UserResponse, versions: List<PackageVersionResponse>, isOwnedByUser: Boolean): PackageResponse {
            return PackageResponse(
                pkg.name,
                author,
                pkg.description,
                pkg.lastUpdated,
                pkg.documentationUrl,
                pkg.repositoryUrl,
                pkg.downloads,
                pkg.latestVersion,
                versions,
                isOwnedByUser
            )
        }
    }
}



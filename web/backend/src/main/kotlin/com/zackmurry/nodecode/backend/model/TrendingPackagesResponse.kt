package com.zackmurry.nodecode.backend.model

data class TrendingPackagesResponse(
    var popular: List<PackagePreviewResponse>,
    var latest: List<PackagePreviewResponse>
)

export interface PackageVersionResponse {
  version: string
  timePublished: number
}

export interface PackageResponse {
  name: string
  author: UserResponse
  description: string
  lastUpdated: number
  documentationUrl?: string
  repositoryUrl?: string
  downloads: number
  latestVersion?: string
  isOwnedByUser: boolean
  versions: PackageVersionResponse[] // sorted: most recent is index 0
}

export interface PackagePreviewResponse {
  name: string
  description: string
  latestVersion?: string
}

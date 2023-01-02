export interface UserPrincipalResponse {
  username: string
  id: string
  enabled: boolean
  accountNonLocked: boolean
  accountNonExpired: boolean
  credentialsNonExpired: boolean
  name: string
  avatarUrl: string
}

interface UserResponse {
  id: string
  username: string
  avatarUrl: string
  timeCreated: number
  numPackages: number
}

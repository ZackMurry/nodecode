const checkSession = async () => {
  try {
    const response = await fetch('/api/v1/users/username')
    const text = await response.text()
    if (!text) {
      window.location.href = '/api/v1/oauth2/code/github'
    }
  } catch {
    window.location.href = '/api/v1/oauth2/code/github'
  }
}

export default checkSession

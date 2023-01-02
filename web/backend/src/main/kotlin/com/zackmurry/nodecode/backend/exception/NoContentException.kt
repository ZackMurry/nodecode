package com.zackmurry.nodecode.backend.exception

import org.springframework.http.HttpStatus
import org.springframework.web.bind.annotation.ResponseStatus

@ResponseStatus(HttpStatus.NO_CONTENT)
class NoContentException : RuntimeException("No content")

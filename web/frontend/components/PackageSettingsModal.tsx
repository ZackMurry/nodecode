import {
  Flex,
  FormControl,
  FormErrorMessage,
  FormHelperText,
  FormLabel,
  InputGroup,
  InputLeftAddon,
  Modal,
  ModalBody,
  ModalCloseButton,
  ModalContent,
  ModalHeader,
  ModalOverlay,
  Text,
  useToast
} from '@chakra-ui/react'
import { useRouter } from 'next/router'
import { FC, FormEvent, useState } from 'react'
import { PackageResponse } from 'types/package'
import { HTTPS_LENGTH } from './CreatePackageForm'
import StyledButton from './StyledButton'
import StyledInput from './StyledInput'

interface Props {
  isOpen: boolean
  onClose: () => void
  pkg: PackageResponse
}

const PackageSettingsModal: FC<Props> = ({ isOpen, onClose, pkg }) => {
  const [description, setDescription] = useState(pkg.description)
  const [descriptionError, setDescriptionError] = useState('')
  const [repositoryUrl, setRepositoryUrl] = useState(pkg.repositoryUrl ?? '')
  const [repositoryError, setRepositoryError] = useState('')
  const [documentationUrl, setDocumentationUrl] = useState(pkg.documentationUrl ?? '')
  const [documentationError, setDocumentationError] = useState('')

  const router = useRouter()
  const toast = useToast()

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault()
    let isError = false
    setDescriptionError(null)
    setRepositoryError(null)
    setDocumentationError(null)
    if (description.length > 200) {
      setDescriptionError('Package description needs to be at most 200 characters.')
      isError = true
    } else if (description.length === 0) {
      setDescriptionError('Package description cannot be empty.')
      isError = true
    }
    if (repositoryUrl.length + HTTPS_LENGTH > 5000) {
      setRepositoryError('Repository URL needs to be at most 5000 characters.')
      isError = true
    }
    if (documentationUrl.length + HTTPS_LENGTH > 5000) {
      setDocumentationError('Repository URL needs to be at most 5000 characters.')
      isError = true
    }
    if (isError) {
      return
    }
    let response: Response
    try {
      response = await fetch(`/api/v1/packages/name/${pkg.name}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          description: description,
          documentationUrl: documentationUrl ? 'https://' + documentationUrl : undefined,
          repositoryUrl: repositoryUrl ? 'https://' + repositoryUrl : undefined
        })
      })
      if (response.ok) {
        router.reload()
      } else if (response.status === 401 || response.status === 403) {
        router.push('/api/v1/oauth2/code/github')
      } else {
        toast({
          title: 'Error',
          description: `Error creating package: ${response.status}`,
          status: 'error',
          duration: 9000,
          isClosable: true
        })
      }
    } catch {
      router.push('/api/v1/oauth2/code/github')
    }
  }

  const handleRepositoryBlur = () => {
    if (repositoryUrl.startsWith('https://')) {
      setRepositoryUrl(repositoryUrl.substring(HTTPS_LENGTH))
    } else if (repositoryUrl.startsWith('http://')) {
      setRepositoryUrl(repositoryUrl.substring(HTTPS_LENGTH - 1))
    }
  }
  const handleDocumentationBlur = () => {
    if (documentationUrl.startsWith('https://')) {
      setDocumentationUrl(documentationUrl.substring(HTTPS_LENGTH))
    } else if (documentationUrl.startsWith('http://')) {
      setDocumentationUrl(documentationUrl.substring(HTTPS_LENGTH - 1))
    }
  }

  return (
    <Modal isOpen={isOpen} onClose={onClose}>
      <ModalOverlay />
      <ModalContent pb='5px'>
        <ModalHeader>Package Settings</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <form onSubmit={handleSubmit}>
            <FormControl isRequired isInvalid={Boolean(descriptionError)}>
              <FormLabel htmlFor='description' mt='15px'>
                Package description
              </FormLabel>
              <StyledInput id='description' value={description} onChange={e => setDescription(e.target.value)} />
              {!descriptionError ? (
                <FormHelperText>Enter a brief (&lt;200 characters) description of your package.</FormHelperText>
              ) : (
                <FormErrorMessage>{descriptionError}</FormErrorMessage>
              )}
              <Text color='gray' fontSize='11px'>
                {description.length} character{description.length !== 1 ? 's' : ''}
              </Text>
            </FormControl>
            <FormControl isInvalid={Boolean(repositoryError)}>
              <FormLabel htmlFor='repository'>Package version</FormLabel>
              <InputGroup>
                <InputLeftAddon children='https://' />
                <StyledInput
                  id='repository'
                  value={repositoryUrl}
                  onChange={e => setRepositoryUrl(e.target.value)}
                  onBlur={handleRepositoryBlur}
                />
              </InputGroup>
              {!repositoryError ? (
                <FormHelperText>Enter a URL for a repository for your package (optional).</FormHelperText>
              ) : (
                <FormErrorMessage>{repositoryError}</FormErrorMessage>
              )}
            </FormControl>
            <FormControl isInvalid={Boolean(documentationError)}>
              <FormLabel htmlFor='documentation' mt='15px'>
                Documentation URL
              </FormLabel>
              <InputGroup>
                <InputLeftAddon children='https://' />
                <StyledInput
                  id='documentation'
                  value={documentationUrl}
                  onChange={e => setDocumentationUrl(e.target.value)}
                  onBlur={handleDocumentationBlur}
                />
              </InputGroup>
              {!documentationError ? (
                <FormHelperText>
                  Enter a URL directing to the documentation page for your package (recommended).
                </FormHelperText>
              ) : (
                <FormErrorMessage>{documentationError}</FormErrorMessage>
              )}
            </FormControl>
            <Flex w='100%' justifyContent='flex-end'>
              <StyledButton type='submit' mt='10px'>
                Update
              </StyledButton>
            </Flex>
          </form>
        </ModalBody>
      </ModalContent>
    </Modal>
  )
}

export default PackageSettingsModal

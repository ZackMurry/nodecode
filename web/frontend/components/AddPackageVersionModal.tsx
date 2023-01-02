import { ExternalLinkIcon } from '@chakra-ui/icons'
import {
  Code,
  Flex,
  FormControl,
  FormErrorMessage,
  FormHelperText,
  FormLabel,
  Link,
  Modal,
  ModalBody,
  ModalCloseButton,
  ModalContent,
  ModalHeader,
  ModalOverlay,
  useToast
} from '@chakra-ui/react'
import { useRouter } from 'next/router'
import { ChangeEvent, FC, FormEvent, useState } from 'react'
import { SEM_VER_REGEX } from './CreateFirstVersionForm'
import StyledButton from './StyledButton'
import StyledInput from './StyledInput'

interface Props {
  isOpen: boolean
  onClose: () => void
  packageName: string
}

const AddPackageVersionModal: FC<Props> = ({ isOpen, onClose, packageName }) => {
  const [version, setVersion] = useState('')
  const [versionError, setVersionError] = useState('')
  const [file, setFile] = useState<Blob | null>(null)
  const [fileError, setFileError] = useState('')

  const router = useRouter()
  const toast = useToast()

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault()

    let isError = false
    setVersionError('')
    setFileError('')
    if (version.length > 16) {
      console.log('ver too long')
      setVersionError('Length of version should not exceed 16 characters.')
      isError = true
    } else if (!SEM_VER_REGEX.test(version)) {
      console.log('test failed')
      setVersionError('Version invalid: must be of the form NUMBER.NUMBER.NUMBER (i.e., /[0-9]+\\.[0-9]+\\.[0-9]+/)')
      isError = true
    }

    if (file === null) {
      setFileError('A file for the version is required.')
      isError = true
    }

    if (isError) {
      return
    }

    const formData = new FormData()
    formData.append('file', file)
    try {
      const response = await fetch(`/api/v1/packages/name/${packageName}/versions/${version}`, {
        method: 'POST',
        body: formData
      })
      if (response.ok) {
        router.reload()
      } else if (response.status === 401 || response.status === 403) {
        router.push('/api/v1/oauth2/code/github')
      } else if (response.status === 409) {
        setVersionError('This version already exists!')
      } else {
        toast({
          title: 'Error',
          description: `Error creating package version: ${response.status}`,
          status: 'error',
          duration: 9000,
          isClosable: true
        })
      }
    } catch {
      router.push('/api/v1/oauth2/code/github')
    }
  }

  const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
    setFile(e.target.files[0])
  }

  return (
    <Modal isOpen={isOpen} onClose={onClose}>
      <ModalOverlay />
      <ModalContent pb='5px'>
        <ModalHeader>Add a new version</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <form onSubmit={handleSubmit}>
            <FormControl isRequired isInvalid={Boolean(versionError)}>
              <FormLabel htmlFor='version'>Package version</FormLabel>
              <StyledInput id='version' value={version} onChange={e => setVersion(e.target.value)} />
              {!versionError ? (
                <FormHelperText>
                  Enter a simple{' '}
                  <Link isExternal href='https://semver.org/'>
                    Semantic Version <ExternalLinkIcon mx='2px' />
                  </Link>{' '}
                  (e.g., 2.4.0) identifier for the first version of this package.
                </FormHelperText>
              ) : (
                <FormErrorMessage>{versionError}</FormErrorMessage>
              )}
            </FormControl>
            <FormControl isRequired isInvalid={Boolean(fileError)}>
              <FormLabel htmlFor='file' mt='15px'>
                Version save file
              </FormLabel>
              <StyledInput
                id='file'
                type='file'
                maxW='250px'
                h='38px'
                pt='5px'
                accept='.nodecode'
                onChange={handleFileChange}
              />
              {!fileError ? (
                <FormHelperText>
                  Upload a save file in the <Code>.nodecode</Code> file format for your package.
                </FormHelperText>
              ) : (
                <FormErrorMessage>{fileError}</FormErrorMessage>
              )}
            </FormControl>
            <Flex w='100%' justifyContent='flex-end'>
              <StyledButton type='submit' mt='10px'>
                Add
              </StyledButton>
            </Flex>
          </form>
        </ModalBody>
      </ModalContent>
    </Modal>
  )
}

export default AddPackageVersionModal

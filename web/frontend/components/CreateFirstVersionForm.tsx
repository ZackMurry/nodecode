import { ExternalLinkIcon } from '@chakra-ui/icons'
import { Code, FormControl, FormErrorMessage, FormHelperText, FormLabel, Link, useToast } from '@chakra-ui/react'
import { useRouter } from 'next/router'
import { ChangeEvent, FC, FormEvent, useState } from 'react'
import StyledButton from './StyledButton'
import StyledInput from './StyledInput'

interface Props {
  onSubmit: () => void
  packageName: string
}

export const SEM_VER_REGEX = new RegExp(/[0-9]+\.[0-9]+\.[0-9]+/)

const CreateFirstVersionForm: FC<Props> = ({ onSubmit, packageName }) => {
  const [version, setVersion] = useState('')
  const [versionError, setVersionError] = useState('')
  const [file, setFile] = useState<Blob | null>(null)
  const [fileError, setFileError] = useState('')

  const router = useRouter()
  const toast = useToast()

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault()

    console.log('submat')
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
      console.log('no file')
      setFileError('A file for the version is required.')
      isError = true
    }

    if (isError) {
      console.log(isError)
      console.log(versionError)
      console.log(fileError)
      return
    }

    console.log('formData')
    const formData = new FormData()
    formData.append('file', file)
    console.log('try')
    try {
      const response = await fetch(`/api/v1/packages/name/${packageName}/versions/${version}`, {
        method: 'POST',
        body: formData
      })
      if (response.ok) {
        onSubmit()
      } else if (response.status === 401 || response.status === 403) {
        router.push('/api/v1/oauth2/code/github')
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
            (e.g., 2.4.0) identifier for the first version of this package. Typical first versions are 0.1.0 or 1.0.0.
          </FormHelperText>
        ) : (
          <FormErrorMessage>{versionError}</FormErrorMessage>
        )}
      </FormControl>
      <FormControl isRequired isInvalid={Boolean(fileError)}>
        <FormLabel htmlFor='file' mt='15px'>
          Version save file
        </FormLabel>
        <StyledInput id='file' type='file' maxW='250px' h='38px' pt='5px' accept='.nodecode' onChange={handleFileChange} />
        {!fileError ? (
          <FormHelperText>
            Upload a save file in the <Code>.nodecode</Code> file format for your package.
          </FormHelperText>
        ) : (
          <FormErrorMessage>{fileError}</FormErrorMessage>
        )}
      </FormControl>
      <StyledButton type='submit' colorScheme='teal' bgColor='tealHero' mt='10px'>
        Add
      </StyledButton>
    </form>
  )
}

export default CreateFirstVersionForm

import {
  Button,
  Flex,
  Modal,
  ModalBody,
  ModalCloseButton,
  ModalContent,
  ModalHeader,
  ModalOverlay,
  Text
} from '@chakra-ui/react'
import { FC } from 'react'

interface Props {
  title: string
  description: string
  isOpen: boolean
  onClose: () => void
  onConfirm: () => void
}

const ConfirmationModal: FC<Props> = ({ title, description, isOpen, onClose, onConfirm }) => (
  <Modal isOpen={isOpen} onClose={onClose}>
    <ModalOverlay />
    <ModalContent pb='5px'>
      <ModalHeader>{title}</ModalHeader>
      <ModalCloseButton />
      <ModalBody>
        <Text mb='15px'>{description}</Text>
        <Flex w='100%' justifyContent='flex-end'>
          <Button variant='outline' onClick={onClose}>
            Cancel
          </Button>
          <Button type='submit' variant='solid' colorScheme='red' onClick={onConfirm} ml='10px'>
            Confirm
          </Button>
        </Flex>
      </ModalBody>
    </ModalContent>
  </Modal>
)

export default ConfirmationModal

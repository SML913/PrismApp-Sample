   M S B A M L   `   `   `  ����  UI!    UI.Views.EmployeeDetailView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      P LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  5      -   5          >�Name$  EmployeeDetail��5      !  Prism.Mvvm.ViewModelLocator   AutoWireViewModel . .��5      � 
�����w� � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      �� 5      $
��Auto �6   .+�5   
   �� $��
First Name��6   $��5 5 5 0q�6%   �� 5   
   $��1��6   $��5 5 5 5q�5      ��5       �� *Employee.FirstName+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5      �� 5   
   $��2��6   $��	Last Name��6   $��5 5 5 0q�61   �� 5   
   $��3��6   $��5 5 5 5q�5      ��5       �� *Employee.LastName+$ TwoWay=�$ PropertyChanged=�5      �� 5   
   $��4��6   $��Company��6   $��5 5 5 0q�6/   �� 5    
   $��5��6    �� DisplayMemberPath$ DisplayMember��5!       �� SelectedValuePath$ Id��5"      $��5 5 5 5q�5$      ��5        �� *	Companies+ �� SelectedValue 5#      �� *Employee.CompanyId+$ TwoWay=�$ PropertyChanged=�5$      �� 5%   
   $��7��6    �� Orientation$ 
Horizontal=�6"   .+�5&      �� $
��Save��6   $��80��6$   $��5 5 5 5q�6/   ��6@   �� *SaveCommand+�� 5'      $��Close��6   $��80��6%   $��5 5 5 5q�60   ��6A   �� *CloseCommand+5(      5*      5+      